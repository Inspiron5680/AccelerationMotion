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
    float replaySpeed;
    float ADJUST_VALUE_MAX = 1;
    float ADJUST_VALUE_MIN = 0.2f;
    [SerializeField] TrajectoryColor trajectoryColor;
    [SerializeField] GameObject replayBall;
    [SerializeField] Material ruledLineMat;
    [SerializeField] TextMesh rateValue; 
    float RULED_LINE_WIDTH = 0.006f;
    bool isPlayingSlowMotion;

    public void Initialize()
    {
        this.UpdateAsObservable()
            .Where(_ => isSelectMode && !isPlayingSlowMotion)
            .Where(_ => OVRInput.GetDown(OVRInput.RawButton.RThumbstickUp) || OVRInput.GetDown(OVRInput.RawButton.LThumbstickUp))
            .Subscribe(_ => 
            {
                if (isRuledLineEnable)
                {
                    WriteRuledLine();
                    selectTrajectory(-1);

                }
                else
                {
                    selectTrajectory(-1);
                }
            });

        this.UpdateAsObservable()
            .Where(_ => isSelectMode && !isPlayingSlowMotion)
            .Where(_ => OVRInput.GetDown(OVRInput.RawButton.RThumbstickDown) || OVRInput.GetDown(OVRInput.RawButton.LThumbstickDown))
            .Subscribe(_ => 
            {
                if (isRuledLineEnable)
                {
                    WriteRuledLine();
                    selectTrajectory(1);

                }
                else
                {
                    selectTrajectory(1);
                }
            });

        AdjustReplaySpeed(2);
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
        isPlayingSlowMotion = true;
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

                elapsedTime += Time.deltaTime * replaySpeed;

                if (instantReplayBall.transform.position.y < 0.0f)
                {
                    isPlayingSlowMotion = false;
                    trajectoryColor.ChangeAlpha(OPAQUE_VALUE, curretTrajectoryID);
                    Destroy(instantReplayBall);
                }
            });
    }

    public void AdjustReplaySpeed(int adjustValue)
    {
        replaySpeed += 0.2f * adjustValue;

        if (replaySpeed < ADJUST_VALUE_MIN)
        {
            replaySpeed = ADJUST_VALUE_MIN;
        }

        if (replaySpeed > ADJUST_VALUE_MAX)
        {
            replaySpeed = ADJUST_VALUE_MAX;
        }

        rateValue.text = $"× {replaySpeed:F1}";
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
                        if (!isRuledLineEnable || !isSelectMode)
                        {
                            Destroy(data.Item2);
                            data.Item3.Dispose();
                        }
                        else
                        {
                            writeLine(data, triPoints[1]);
                        }
                    }))
                .ToArray();
        }

        LineRenderer setLineData(LineRenderer lineRenderer)
        {
            lineRenderer.material = ruledLineMat;
            lineRenderer.startWidth = RULED_LINE_WIDTH;
            lineRenderer.endWidth = RULED_LINE_WIDTH;

            return lineRenderer;
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
