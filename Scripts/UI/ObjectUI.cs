using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class ObjectUI : MonoBehaviour
{
    [SerializeField] SelectMode selectMode;
    [SerializeField] SwitchPlayerModeUI switchPlayerModeUI;
    [SerializeField] GameObject target;
    bool lastIsGrabbed;

    // Start is called before the first frame update
    void Start()
    {
        selectMode.Initialize();
        switchPlayerModeUI.Initialize();

        var grabbable = GetComponent<OVRGrabbable>();

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                if (!grabbable.isGrabbed && lastIsGrabbed)
                {
                    lookAtPlayer();
                }

                lastIsGrabbed = grabbable.isGrabbed;
            });
    }

    void lookAtPlayer()
    {
        float speed = 0.1f;
        var relativePosition 
            = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z);
        var rotation = Quaternion.LookRotation(relativePosition);

        var disposable = new SingleAssignmentDisposable();
        disposable.Disposable = this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed);
                if (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.x, rotation.eulerAngles.x)) < 0.1 &&
                    Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, rotation.eulerAngles.y)) < 0.1 &&
                    Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.z, rotation.eulerAngles.z)) < 0.1)
                {
                    disposable.Dispose();
                }
            });
    }

}
