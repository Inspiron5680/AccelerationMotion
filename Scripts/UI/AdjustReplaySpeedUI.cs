using UnityEngine;

public class AdjustReplaySpeedUI : MonoBehaviour, VRUI
{
    enum AdjustDirection
    {
        Up, Down
    }

    [SerializeField] AdjustDirection adjustDirection;
    [SerializeField] SelectMode selectMode;

    public void Receiver()
    {
        switch (adjustDirection)
        {
            case AdjustDirection.Up:
                selectMode.AdjustReplaySpeed(1);
                break;
            case AdjustDirection.Down:
                selectMode.AdjustReplaySpeed(-1);
                break;
        }
    }

    public void Reaction()
    {
        throw new System.NotImplementedException();
    }
}
