using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Pitching : MonoBehaviour
{
    OVRGrabbable grabbable;
    bool lastIsGrabbed;
    bool doThrow;
    Vector3 throwVelocity;
    Vector3 throwPosition;
    float elapsedTime;
    float GRAVITY = 9.81f;

    void Start()
    {
        Initialize();
    }

    /// <summary>
    /// Pitchingの初期化
    /// </summary>
    public void Initialize()
    {
        grabbable = GetComponent<OVRGrabbable>();

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
        doThrow = true;
    }

    void throwBall()
    {
        var coordinateX = throwPosition.x + throwVelocity.x * elapsedTime;
        var coordinateY = throwPosition.y + throwVelocity.y * elapsedTime - GRAVITY * Mathf.Pow(elapsedTime, 2) / 2;
        var coordinateZ = throwPosition.z + throwVelocity.z * elapsedTime;

        gameObject.transform.position = new Vector3(coordinateX, coordinateY, coordinateZ);
    }
}
