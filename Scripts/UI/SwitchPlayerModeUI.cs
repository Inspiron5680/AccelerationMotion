﻿using UnityEngine;

public class SwitchPlayerModeUI : MonoBehaviour, VRUI
{
    public enum PlayerMode
    {
        Pitching, Observe
    }
    public PlayerMode CurrentMode { get; private set; }

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
        CurrentMode = PlayerMode.Pitching;
    }

    public void Receiver()
    {
        ChangePlayerMode();
    }

    public void ChangePlayerMode()
    {
        switch (CurrentMode)
        {
            case PlayerMode.Pitching:
                pitchingUI.SetActive(false);
                observeUI.SetActive(true);
                CurrentMode = PlayerMode.Observe;
                break;
            case PlayerMode.Observe:
                pitchingUI.SetActive(true);
                observeUI.SetActive(false);
                CurrentMode = PlayerMode.Pitching;
                break;
        }
    }
}
