using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class Pitching : MonoBehaviour
{
    Trajectory trajectory;
    bool lastIsGrabbed;
    bool doThrow;
    Vector3 throwVelocity;
    Vector3 throwPosition;
    float elapsedTime;
    readonly float GRAVITY = 9.81f;
    [SerializeField] GameObject ball;
    GameObject trajectoryParent;
    SwitchPlayerModeUI switchPlayerMode;

    void Start()
    {
        Initialize();
    }

    /// <summary>
    /// Pitchingの初期化
    /// </summary>
    public void Initialize()
    {
        var grabbable = GetComponent<OVRGrabbable>();
        trajectory = GetComponent<Trajectory>();
        switchPlayerMode = GameObject.Find("PlayerModeSwitcherUI").GetComponent<SwitchPlayerModeUI>();


        //NOTE:ボールをつかんだ瞬間ストックを行うストリーム
        this.UpdateAsObservable()
            .Where(_ => grabbable.isGrabbed && !lastIsGrabbed)
            .Subscribe(_ =>
            {
                if (switchPlayerMode.CurrentMode == SwitchPlayerModeUI.PlayerMode.Select)
                {
                    switchPlayerMode.ChangePlayerMode();
                }

                TrajectoryControl.ResetTurnAxis();
                gameObject.transform.parent = null;
                trajectory.Retry();
            });

        //NOTE:投げられたことを検知して軌跡の生成を始めるストリーム
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                if (!grabbable.isGrabbed && lastIsGrabbed)
                {
                    startThrowing();
                }

                lastIsGrabbed = grabbable.isGrabbed;
            });

        //NOTE:軌跡生成ストリーム
        this.UpdateAsObservable()
            .Where(_ => doThrow)
            .Subscribe(_ => 
            {
                throwBall();
                elapsedTime += Time.deltaTime;
            });

        //NOTE:投げられたボールを削除してリスポンさせるストリーム
        this.UpdateAsObservable()
            .Where(_ => doThrow)
            .Where(_ => gameObject.transform.position.y < 0)
            .Subscribe(_ =>
            {
                respown();
                Destroy(gameObject);
            });
    }

    void startThrowing()
    {
        var rigidBody = GetComponent<Rigidbody>();
        throwVelocity = rigidBody.velocity;
        throwPosition = gameObject.transform.position;
        trajectoryParent = trajectory.CreateParent(throwPosition);
        doThrow = true;
    }

    void throwBall()
    {
        var coordinateX = throwPosition.x + throwVelocity.x * elapsedTime;
        var coordinateY = throwPosition.y + throwVelocity.y * elapsedTime - GRAVITY * Mathf.Pow(elapsedTime, 2) / 2;
        var coordinateZ = throwPosition.z + throwVelocity.z * elapsedTime;

        var position = new Vector3(coordinateX, coordinateY, coordinateZ);

        trajectory.CreateTrajectory(position, elapsedTime);
        gameObject.transform.position = position;
    }

    void respown()
    {
        var instantBall = Instantiate(ball, Vector3.zero, Quaternion.identity);
        instantBall.name = ball.name;
        var trajectoryComponent = instantBall.GetComponent<Trajectory>();
        trajectoryComponent.LastTrajectoryData = Tuple.Create(trajectoryParent, throwVelocity);

        StockTrajectoryUI.Trajectory = trajectoryComponent;

        var objectUI = FindObjectOfType<ObjectUI>();
        instantBall.transform.parent = objectUI.transform;
        instantBall.transform.localPosition = new Vector3(0, 0.4f, -0.1f);
    }
}
