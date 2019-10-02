using UnityEngine;

public class ChangeSelectModeUI : MonoBehaviour, VRUI
{
    [SerializeField] SelectMode selectMode;

    public void Receiver()
    {
        selectMode.ChangeSelectMode();
    }
}
