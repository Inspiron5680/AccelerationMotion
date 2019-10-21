using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrajectoryUI : MonoBehaviour, VRUI
{
    public void Reaction()
    {
        return;
    }

    public void Receiver()
    {
        if (TrajectoryControl.TrajectoryParents.Count == 0)
        {
            return;
        }

        foreach(GameObject parent in TrajectoryControl.TrajectoryParents)
        {
            Destroy(parent);
        }

        TrajectoryControl.TrajectoryParents.Clear();
    }
}
