using UnityEngine;

public class SwitchPlayerModeUI : MonoBehaviour, VRUI
{
    enum PlayerMode
    {
        Pitching, Observe
    }

    PlayerMode currentMode;

    [SerializeField] GameObject pitchingUI;
    [SerializeField] GameObject observeUI;

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        pitchingUI.SetActive(true);
        observeUI.SetActive(false);
        currentMode = PlayerMode.Pitching;
    }

    public void Receiver()
    {
        switch (currentMode)
        {
            case PlayerMode.Pitching:
                pitchingUI.SetActive(false);
                observeUI.SetActive(true);
                currentMode = PlayerMode.Observe;
                break;
            case PlayerMode.Observe:
                pitchingUI.SetActive(true);
                observeUI.SetActive(false);
                currentMode = PlayerMode.Pitching;
                break;
        }
    }
}
