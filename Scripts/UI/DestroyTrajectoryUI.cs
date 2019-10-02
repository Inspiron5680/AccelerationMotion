using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrajectoryUI : MonoBehaviour, VRUI
{
    public void Receiver()
    {
        foreach(GameObject parent in TrajectoryControl.TrajectoryParents)
        {
            Destroy(parent);
        }

        TrajectoryControl.TrajectoryParents.Clear();
    }
}
