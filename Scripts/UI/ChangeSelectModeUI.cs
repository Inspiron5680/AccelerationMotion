using UnityEngine;

public class ChangeSelectModeUI : MonoBehaviour, VRUI
{
    [SerializeField] SelectMode selectMode;

    public void Reaction()
    {
        throw new System.NotImplementedException();
    }

    public void Receiver()
    {
        selectMode.ChangeSelectMode();
    }
}
