using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockTrajectoryUI : MonoBehaviour, ILaserSelectReceiver
{
    public static Trajectory Trajectory { private get; set; }

    public void LaserSelectReceiver()
    {
        if (Trajectory == null)
        {
            return;
        }

        Trajectory.StockTrajectory();
    }
}
