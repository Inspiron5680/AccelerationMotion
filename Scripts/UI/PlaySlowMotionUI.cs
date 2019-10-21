using UnityEngine;
using System.Threading.Tasks;

public class PlaySlowMotionUI : MonoBehaviour, VRUI
{
    [SerializeField] SelectMode selectMode;
    int delayTime = 250;

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

        blazeThrow();
    }

    async void blazeThrow()
    {
        for(int count = 0; count < 3; count++)
        {
            selectMode.PlaySlowMotion();
            await Task.Delay(delayTime);
        }
    }
}
