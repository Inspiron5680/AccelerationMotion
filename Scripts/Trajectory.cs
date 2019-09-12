using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] GameObject trajectoryParent;
    [SerializeField] GameObject trajectoryBall;
    GameObject instantTrajectoryParent;
    readonly float CREATE_INTERVAL = 0.1f;
    float lastCreateTime;
    

    public void Initialize(Vector3 throwPosition)
    {
        instantTrajectoryParent = Instantiate(trajectoryParent, throwPosition, Quaternion.identity);
    }

    public void CreateTrajectory(Vector3 ballPosition,float elapsedTime)
    {
        var differenceTime = elapsedTime - lastCreateTime;
        if (differenceTime < CREATE_INTERVAL && elapsedTime != 0.0f)
        {
            return;
        }
        var trajectory = Instantiate(trajectoryBall, ballPosition, Quaternion.identity);
        trajectory.transform.parent = instantTrajectoryParent.transform;
        lastCreateTime = elapsedTime;
    }
}
