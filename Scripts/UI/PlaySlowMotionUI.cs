using UnityEngine;

public class PlaySlowMotionUI : MonoBehaviour, ILaserSelectReceiver
{
    [SerializeField] SelectMode selectMode;

    public void LaserSelectReceiver()
    {
        selectMode.PlaySlowMotion();
    }
}
