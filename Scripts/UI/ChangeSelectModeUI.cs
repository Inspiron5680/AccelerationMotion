using UnityEngine;

public class ChangeSelectModeUI : MonoBehaviour, VRUI
{
    [SerializeField] SelectMode selectMode;

    public void Reaction()
    {
        return;
    }

    public void Receiver()
    {
        selectMode.ChangeSelectMode();
    }
}
