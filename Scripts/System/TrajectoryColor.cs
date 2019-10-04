using System.Linq;
using UnityEngine;

public class TrajectoryColor : MonoBehaviour
{
    public void ChangeAlpha(float alphaValue,int index)
    {
        TrajectoryControl.TrajectoryParents[index].GetComponentsInChildren<MeshRenderer>()
            .Where(renderer => renderer.gameObject.tag == "Trajectory")
            .Select(renderer => renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, alphaValue))
            .ToArray();
    }

    public void ChangeAlpha(float alphaValue)
    {
        for(int count = 0; count < TrajectoryControl.TrajectoryParents.Count; count++)
        {
            ChangeAlpha(alphaValue, count);
        }
    }
}
