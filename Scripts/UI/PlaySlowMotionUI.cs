using UnityEngine;

public class PlaySlowMotionUI : MonoBehaviour, VRUI
{
    [SerializeField] SelectMode selectMode;

    public void Receiver()
    {
        selectMode.PlaySlowMotion();
    }
}
