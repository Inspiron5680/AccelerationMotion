using UnityEngine;

public class BallColor
{
    static Color[] colors = new Color[]
    {
        Color.blue,
        Color.cyan,
        Color.green,
        Color.magenta,
        Color.red,
        Color.yellow
    };

    static int currentColorIndex;

    public static Color CurrentballColor { get { return colors[currentColorIndex]; } }

    public static void ChangeBallColor()
    {
        currentColorIndex++;
        if (currentColorIndex == colors.Length)
        {
            currentColorIndex = 0;
        }
    }
}
