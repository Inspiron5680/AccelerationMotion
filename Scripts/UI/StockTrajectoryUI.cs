using UnityEngine;

public class StockTrajectoryUI : MonoBehaviour, VRUI
{
    public static Trajectory Trajectory { private get; set; }

    public void Receiver()
    {
        if (Trajectory == null)
        {
            return;
        }

        Trajectory.StockTrajectory();
        Destroy(transform.parent.gameObject);
    }
}
