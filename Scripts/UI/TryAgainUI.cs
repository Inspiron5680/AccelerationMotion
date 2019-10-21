using UnityEngine;
using System.Linq;

public class TryAgainUI : MonoBehaviour, VRUI
{
    public void Reaction()
    {
        throw new System.NotImplementedException();
    }

    public void Receiver()
    {
        var trajectoryParents = FindObjectsOfType<TrajectoryControl>();
        if (trajectoryParents.Length == 0)
        {
            return;
        }

        var lastThrowTrajetory = trajectoryParents
            .FirstOrDefault(trajectoryParent => trajectoryParent.transform.parent == null);

        if (lastThrowTrajetory != null)
        {
            Destroy(lastThrowTrajetory.transform.gameObject);
        }
    }
}
