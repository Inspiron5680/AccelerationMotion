using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;

public class TrajectoryControl : MonoBehaviour
{
    public Vector3 ThrowVelocity { get; set; }
    Vector3 throwPosition = new Vector3(-2.0f, 2.0f, 0.0f);
    public static List<GameObject> TrajectoryParents = new List<GameObject>();
    static GameObject turnAxis;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (turnAxis != null)
        {
            return;
        }

        turnAxis = GameObject.Find("TurnAxis");
        turnAxis.UpdateAsObservable()
            .Subscribe(_ =>
            {
                var stickAxis = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);
                turnAxis.transform.Rotate(0, Mathf.Round(stickAxis.x), 0);
            });

        turnAxis.UpdateAsObservable()
            .Subscribe(_ =>
            {
                var stickAxis = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
                turnAxis.transform.Rotate(0, Mathf.Round(stickAxis.x), 0);
            });
    }

    public void StockTrajectory()
    {
        alineTrajectory();
        transform.position = throwPosition;
        transform.parent = turnAxis.transform;
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
