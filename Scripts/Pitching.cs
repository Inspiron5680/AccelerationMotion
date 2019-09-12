using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Pitching : MonoBehaviour
{
    Trajectory trajectory;
    bool lastIsGrabbed;
    bool doThrow;
    Vector3 throwVelocity;
    Vector3 throwPosition;
    float elapsedTime;
    readonly float GRAVITY = 9.81f;

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

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                if (!grabbable.isGrabbed && lastIsGrabbed)
                {
                    startThrowing();
                }

                lastIsGrabbed = grabbable.isGrabbed;
            });

        this.UpdateAsObservable()
            .Where(_ => doThrow)
            .Subscribe(_ => 
            {
                throwBall();
                elapsedTime += Time.deltaTime;
            });

        this.UpdateAsObservable()
            .Where(_ => doThrow)
            .Where(_ => gameObject.transform.position.y < 0)
            .Subscribe(_ => Destroy(gameObject));
    }

    void startThrowing()
    {
        var rigidBody = GetComponent<Rigidbody>();
        throwVelocity = rigidBody.velocity;
        throwPosition = gameObject.transform.position;
        trajectory.Initialize(throwPosition);
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
}
