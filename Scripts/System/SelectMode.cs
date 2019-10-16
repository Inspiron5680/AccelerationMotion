using System.Linq;
using UnityEngine;
using UniRx.Triggers;
using UniRx;
using System;

public class SelectMode : MonoBehaviour
{
    int curretTrajectoryID;
    float FADE_VALUE = 0.1f;
    float OPAQUE_VALUE = 1.0f;
    bool isSelectMode;
    bool isRuledLineEnable;
    int replaySpeed;
    int ADJUST_VALUE_MAX = 5;
    int ADJUST_VALUE_MIN = 1;
    [SerializeField] TrajectoryColor trajectoryColor;
    [SerializeField] GameObject replayBall;
    [SerializeField] Material ruledLineMat;
    float RULED_LINE_WIDTH = 0.006f;

    public void Initialize()
    {
        this.UpdateAsObservable()
            .Where(_ => isSelectMode)
            .Where(_ => OVRInput.GetDown(OVRInput.RawButton.RThumbstickUp) || OVRInput.GetDown(OVRInput.RawButton.LThumbstickUp))
            .Subscribe(_ => selectTrajectory(-1));

        this.UpdateAsObservable()
            .Where(_ => isSelectMode)
            .Where(_ => OVRInput.GetDown(OVRInput.RawButton.RThumbstickDown) || OVRInput.GetDown(OVRInput.RawButton.LThumbstickDown))
            .Subscribe(_ => selectTrajectory(1));

        replaySpeed = 3;
    }

    public void ChangeSelectMode()
    {
        isSelectMode = !isSelectMode;

        if (TrajectoryControl.TrajectoryParents.Count == 0)
        {
            return;
        }

        if (isSelectMode)
        {
            trajectoryColor.ChangeAlpha(FADE_VALUE);
            trajectoryColor.ChangeAlpha(OPAQUE_VALUE, 0);
            curretTrajectoryID = 0;
            isRuledLineEnable = false;
        }
        else
        {
            trajectoryColor.ChangeAlpha(OPAQUE_VALUE);
        }
    }

    void selectTrajectory(int shiftIndex)
    {
        trajectoryColor.ChangeAlpha(FADE_VALUE, curretTrajectoryID);

        curretTrajectoryID = (curretTrajectoryID + shiftIndex) % TrajectoryControl.TrajectoryParents.Count;
        if (curretTrajectoryID < 0)
        {
            curretTrajectoryID += TrajectoryControl.TrajectoryParents.Count;
        }

        trajectoryColor.ChangeAlpha(OPAQUE_VALUE, curretTrajectoryID);
    }

    public void PlaySlowMotion()
    {
        var gravity = 9.81f;
        var elapsedTime = 0.0f;
        trajectoryColor.ChangeAlpha(FADE_VALUE, curretTrajectoryID);
        var replayVelocity = TrajectoryControl.TrajectoryParents[curretTrajectoryID].GetComponent<TrajectoryControl>().ThrowVelocity;

        var instantReplayBall = Instantiate(replayBall, TrajectoryControl.TrajectoryParents[curretTrajectoryID].transform.position, TrajectoryControl.TrajectoryParents[curretTrajectoryID].transform.rotation);
        instantReplayBall.transform.parent = TrajectoryControl.TrajectoryParents[curretTrajectoryID].transform;
        instantReplayBall.tag = "Untagged";

        instantReplayBall.UpdateAsObservable()
            .Subscribe(_ =>
            {
                var coordinateX = replayVelocity.x * elapsedTime;
                var coordinateY = replayVelocity.y * elapsedTime - gravity * Mathf.Pow(elapsedTime, 2) / 2;
                var coordinateZ = replayVelocity.z * elapsedTime;

                var position = new Vector3(coordinateX, coordinateY, coordinateZ);
                instantReplayBall.transform.localPosition = position;

                elapsedTime += Time.deltaTime / replaySpeed;

                if (instantReplayBall.transform.position.y < 0.0f)
                {
                    trajectoryColor.ChangeAlpha(OPAQUE_VALUE, curretTrajectoryID);
                    Destroy(instantReplayBall);
                }
            });
    }

    public void AdjustReplaySpeed(int adjustValue)
    {
        replaySpeed += adjustValue;

        if (replaySpeed < ADJUST_VALUE_MIN)
        {
            replaySpeed = ADJUST_VALUE_MIN;
        }

        if (replaySpeed > ADJUST_VALUE_MAX)
        {
            replaySpeed = ADJUST_VALUE_MAX;
        }
    }

    public void WriteRuledLine()
    {
        var trajectoryBalls = TrajectoryControl.TrajectoryParents[curretTrajectoryID].GetComponentsInChildren<Transform>();
        isRuledLineEnable = !isRuledLineEnable;

        if (!trajectoryBalls[0].GetComponent<LineRenderer>())
        {
            var triPoints = getTriPoints();
            trajectoryBalls
                .Select(transform => Tuple.Create(transform.gameObject, setLineData(transform.gameObject.AddComponent<LineRenderer>()), new SingleAssignmentDisposable()))
                .Select(data => data.Item3.Disposable = data.Item1.UpdateAsObservable()
                    .Subscribe(_ =>
                    {
                        writeLine(data, triPoints[1]);

                        if (!isRuledLineEnable || !isSelectMode)
                        {
                            deleteLine();
                            data.Item3.Dispose();
                        }
                    }))
                .ToArray();
        }
        else
        {
            deleteLine();
        }

        LineRenderer setLineData(LineRenderer lineRenderer)
        {
            lineRenderer.material = ruledLineMat;
            lineRenderer.startWidth = RULED_LINE_WIDTH;
            lineRenderer.endWidth = RULED_LINE_WIDTH;

            return lineRenderer;
        }

        void deleteLine()
        {
            foreach (Transform ball in trajectoryBalls)
            {
                Destroy(ball.GetComponent<LineRenderer>());
            }
        }

        Transform[] getTriPoints()
        {
            var startPoint = trajectoryBalls[0];
            var middlePoint = trajectoryBalls
                .OrderByDescending(transform => transform.position.y)
                .First();
            var endPoint = trajectoryBalls[trajectoryBalls.Length - 1];

            return new Transform[] { startPoint, middlePoint, endPoint };
        }

        void writeLine(Tuple<GameObject,LineRenderer,SingleAssignmentDisposable> data,Transform end)
        {
            data.Item2.enabled = true;
            data.Item2.positionCount = 3;

            var positions = new Vector3[] {new Vector3(data.Item1.transform.position.x, 0, data.Item1.transform.position.z)
                ,data.Item1.transform.position
                ,new Vector3(end.position.x, data.Item1.transform.position.y, end.position.z)};

            data.Item2.SetPositions(positions);
        }
    }
}
