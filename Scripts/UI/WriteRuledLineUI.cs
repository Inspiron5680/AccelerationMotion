using UnityEngine;

public class WriteRuledLineUI : MonoBehaviour,VRUI
{
    [SerializeField] SelectMode selectMode;

    public void Reaction()
    {
        return;
    }

    public void Receiver()
    {
        if (!selectMode)
        {
            return;
        }

        if (TrajectoryControl.TrajectoryParents.Count == 0)
        {
            return;
        }

        selectMode.WriteRuledLine();
    }
}
