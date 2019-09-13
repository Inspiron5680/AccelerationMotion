using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] GameObject trajectoryParent;
    [SerializeField] GameObject trajectoryBall;
    GameObject instantTrajectoryParent;
    readonly float CREATE_INTERVAL = 0.1f;
    float lastCreateTime;
    /// <summary>
    /// 前回の投球で生成したTrajectoryParent
    /// </summary>
    public GameObject LastTrajectoryParent { private get; set; }
    
    /// <summary>
    /// TrajectoryParent生成
    /// </summary>
    /// <param name="throwPosition"></param>
    public GameObject CreateParent(Vector3 throwPosition)
    {
        instantTrajectoryParent = Instantiate(trajectoryParent, throwPosition, Quaternion.identity);
        instantTrajectoryParent.AddComponent(typeof(TrajectoryControl));
        return instantTrajectoryParent;
    }

    /// <summary>
    /// 軌跡を生成する
    /// </summary>
    /// <param name="ballPosition">現在のボールの座標</param>
    /// <param name="elapsedTime">ボールを投げてからの経過時間</param>
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

    /// <summary>
    /// 生成済みの軌跡をストックさせるs
    /// </summary>
    public void StockTrajectory()
    {
        if (!LastTrajectoryParent)
        {
            return;
        }
        
        LastTrajectoryParent.GetComponent<TrajectoryControl>().StockTrajectory();
    }
}
