using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrajectoryUI : MonoBehaviour, ILaserSelectReceiver
{
    public void LaserSelectReceiver()
    {
        foreach(GameObject parent in TrajectoryControl.TrajectoryParents)
        {
            Destroy(parent);
        }

        TrajectoryControl.TrajectoryParents.Clear();
    }
}
