using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryControl : MonoBehaviour
{
    public Vector3 ThrowVelocity { private get; set; }
    public static List<GameObject> TrajectoryParents = new List<GameObject>();

    public void StockTrajectory()
    {
        alineTrajectory();
        transform.position = new Vector3(-2.0f, 2.0f, 0.0f);
        TrajectoryParents.Add(gameObject);
    }

    void alineTrajectory()
    {
        var radian = Mathf.Atan2(ThrowVelocity.x, ThrowVelocity.z);
        //NOTE:ラジアン(radian)から度(degree)を求める計算式 
        var degree = radian * 180.0f / Mathf.PI;

        transform.rotation = Quaternion.Euler(0.0f, -1 * degree, 0.0f);
    }
}
