using UnityEngine;

public class ChangeSelectModeUI : MonoBehaviour, ILaserSelectReceiver
{
    [SerializeField] SelectMode selectMode;

    public void LaserSelectReceiver()
    {
        selectMode.ChangeSelectMode();
    }
}
