using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowModeUI : MonoBehaviour, VRUI
{
    public enum Mode
    {
        Normal, Vartical, Horizontal
    }

    [SerializeField] Mode mode;
    [SerializeField] Transform outLine;
    public static Mode CurrentMode { get; private set; }

    public void Reaction()
    {
        return;
    }

    public void Receiver()
    {
        var myThrowBall = GameObject.Find("ObjectUI/ThrowBall");

        if (!myThrowBall)
        {
            return;
        }

        switch (mode)
        {
            case Mode.Vartical:
                outLine.localPosition = new Vector3(0, -0.0237f, 0.001f);
                CurrentMode = Mode.Vartical;
                break;
            case Mode.Horizontal:
                outLine.localPosition = new Vector3(0.1239f, -0.0237f, 0.001f);
                CurrentMode = Mode.Horizontal;
                break;
            case Mode.Normal:
                outLine.localPosition = new Vector3(-0.1239f, -0.0237f, 0.001f);
                CurrentMode = Mode.Normal;
                break;
        }

        myThrowBall.GetComponent<Pitching>().Throw = (Pitching.ThrowMode)mode;
    }
}
