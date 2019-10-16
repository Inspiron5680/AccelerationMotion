using UnityEngine;

public class SwitchPlayerModeUI : MonoBehaviour, VRUI
{
    public enum PlayerMode
    {
        Pitching, Select
    }
    public PlayerMode CurrentMode { get; private set; }

    [SerializeField] GameObject pitchingUI;
    [SerializeField] GameObject selectUI;
    GameObject pitchingBall;

    public void Initialize()
    {
        pitchingUI.SetActive(true);
        selectUI.SetActive(false);
        CurrentMode = PlayerMode.Pitching;
    }

    public void Receiver()
    {
        ChangePlayerMode();
    }

    public void ChangePlayerMode()
    {
        if (pitchingBall == null)
        {
            pitchingBall = FindObjectOfType<Pitching>().gameObject;
        }

        switch (CurrentMode)
        {
            case PlayerMode.Pitching:
                pitchingUI.SetActive(false);
                selectUI.SetActive(true);
                pitchingBall.SetActive(false);
                selectUI.GetComponent<SelectMode>().ChangeSelectMode();
                CurrentMode = PlayerMode.Select;
                break;
            case PlayerMode.Select:
                TrajectoryControl.ResetTurnAxis();
                pitchingUI.SetActive(true);
                selectUI.SetActive(false);
                pitchingBall.SetActive(true);
                selectUI.GetComponent<SelectMode>().ChangeSelectMode();
                CurrentMode = PlayerMode.Pitching;
                break;
        }
    }
}
