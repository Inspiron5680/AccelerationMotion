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

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        this.UpdateAsObservable()
            .Where(_ => OVRInput.GetDown(OVRInput.RawButton.RThumbstickUp) || OVRInput.GetDown(OVRInput.RawButton.LThumbstickUp))
            .Subscribe(_ => selectTrajectory(-1));

        this.UpdateAsObservable()
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
            changeAlpha(OPAQUE_VALUE);
        }
        else
        {
            changeAlpha(FADE_VALUE);
            TrajectoryControl.TrajectoryParents[0].GetComponentsInChildren<MeshRenderer>()
                .Select(renderer => renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, OPAQUE_VALUE))
                .ToArray();
        }

        IsSelectMode = !IsSelectMode;
    }

    void changeAlpha(float alphaValue)
    {
        TrajectoryControl.TrajectoryParents
            .SelectMany(parent => parent.GetComponentsInChildren<MeshRenderer>())
            .Select(renderer => renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, alphaValue))
            .ToArray();
    }

    void selectTrajectory(int shiftIndex)
    {
        TrajectoryControl.TrajectoryParents[curretTrajectoryID].GetComponentsInChildren<MeshRenderer>()
                .Select(renderer => renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, FADE_VALUE))
                .ToArray();

        curretTrajectoryID = (curretTrajectoryID + shiftIndex) % TrajectoryControl.TrajectoryParents.Count;
        if (curretTrajectoryID < 0)
        {
            curretTrajectoryID += TrajectoryControl.TrajectoryParents.Count;
        }

        TrajectoryControl.TrajectoryParents[curretTrajectoryID].GetComponentsInChildren<MeshRenderer>()
                .Select(renderer => renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, OPAQUE_VALUE))
                .ToArray();
    }
}
