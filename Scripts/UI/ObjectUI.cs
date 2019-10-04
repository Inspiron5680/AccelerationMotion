using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectUI : MonoBehaviour
{
    [SerializeField] SelectMode selectMode;
    [SerializeField] SwitchPlayerModeUI switchPlayerModeUI;

    // Start is called before the first frame update
    void Start()
    {
        selectMode.Initialize();
        switchPlayerModeUI.Initialize();
    }

}
