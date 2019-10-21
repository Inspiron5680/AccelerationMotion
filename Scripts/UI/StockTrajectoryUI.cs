using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockTrajectoryUI : MonoBehaviour, VRUI
{
    public static Trajectory Trajectory { private get; set; }

    public void Reaction()
    {
        return;
    }

    public void Receiver()
    {
        if (Trajectory == null)
        {
            return;
        }

        Trajectory.StockTrajectory();
    }
}
