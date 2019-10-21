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
    [SerializeField] TextMesh textMesh;
    const string selectModeText = "観察モード";
    const string throwModeText = "投球モード";


    public void Initialize()
    {
        pitchingUI.SetActive(true);
        selectUI.SetActive(false);
        CurrentMode = PlayerMode.Pitching;
        textMesh.text = selectModeText;
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
                textMesh.text = throwModeText;
                break;
            case PlayerMode.Select:
                TrajectoryControl.ResetTurnAxis();
                pitchingUI.SetActive(true);
                selectUI.SetActive(false);
                pitchingBall.SetActive(true);
                selectUI.GetComponent<SelectMode>().ChangeSelectMode();
                CurrentMode = PlayerMode.Pitching;
                textMesh.text = selectModeText;
                break;
        }
    }

    public void Reaction()
    {
        throw new System.NotImplementedException();
    }
}
