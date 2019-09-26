using System.Linq;
using UnityEngine;
using UniRx.Triggers;
using UniRx;

public class SelectMode : MonoBehaviour
{
    int curretTrajectoryID;
    float FADE_VALUE = 0.3f;
    float OPAQUE_VALUE = 1.0f;
    public bool IsSelectMode { get; private set; }
    [SerializeField] TrajectoryColor trajectoryColor;
    [SerializeField] PlaySlowMotionUI playSlowMotionUI;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        this.UpdateAsObservable()
            .Where(_ => IsSelectMode)
            .Where(_ => OVRInput.GetDown(OVRInput.RawButton.RThumbstickUp) || OVRInput.GetDown(OVRInput.RawButton.LThumbstickUp))
            .Subscribe(_ => selectTrajectory(-1));

        this.UpdateAsObservable()
            .Where(_ => IsSelectMode)
            .Where(_ => OVRInput.GetDown(OVRInput.RawButton.RThumbstickDown) || OVRInput.GetDown(OVRInput.RawButton.LThumbstickDown))
            .Subscribe(_ => selectTrajectory(1));
    }

    public void ChangeSelectMode()
    {
        if (TrajectoryControl.TrajectoryParents.Count == 0)
        {
            return;
        }

        if (IsSelectMode)
        {
            trajectoryColor.ChangeAlpha(OPAQUE_VALUE);
            playSlowMotionUI.gameObject.SetActive(false);
        }
        else
        {
            trajectoryColor.ChangeAlpha(FADE_VALUE);
            trajectoryColor.ChangeAlpha(OPAQUE_VALUE, 0);
            playSlowMotionUI.gameObject.SetActive(true);
        }

        IsSelectMode = !IsSelectMode;
    }

    void selectTrajectory(int shiftIndex)
    {
        trajectoryColor.ChangeAlpha(FADE_VALUE, curretTrajectoryID);

        curretTrajectoryID = (curretTrajectoryID + shiftIndex) % TrajectoryControl.TrajectoryParents.Count;
        if (curretTrajectoryID < 0)
        {
            curretTrajectoryID += TrajectoryControl.TrajectoryParents.Count;
        }

        trajectoryColor.ChangeAlpha(OPAQUE_VALUE, curretTrajectoryID);
    }
}
