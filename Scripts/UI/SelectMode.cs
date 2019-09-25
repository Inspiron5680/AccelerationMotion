using System.Linq;
using UnityEngine;

public class SelectMode : MonoBehaviour, ILaserSelectReceiver
{
    int curretTrajectoryID;
    float FADE_VALUE = 0.3f;
    float OPAQUE_VALUE = 1.0f;
    public bool IsSelectMode { get; private set; }

    public void LaserSelectReceiver()
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
}
