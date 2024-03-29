﻿using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;
using System.Collections.Generic;

public class Pitching : MonoBehaviour
{
    Trajectory trajectory;
    bool lastIsGrabbed;
    bool doThrow;
    Vector3 throwVelocity;
    Vector3 throwPosition;
    float elapsedTime;
    readonly float GRAVITY = 9.81f;
    [SerializeField] GameObject ball;
    GameObject trajectoryParent;
    SwitchPlayerModeUI switchPlayerMode;

    List<Vector3> lastVelocitis = new List<Vector3>();
    [SerializeField] int velocityLength;

    public enum ThrowMode
    {
        Normal, Vartical, Horizontal
    }

    public ThrowMode Throw { private get; set; } 

    void Start()
    {
        Initialize();
    }

    /// <summary>
    /// Pitchingの初期化
    /// </summary>
    public void Initialize()
    {
        var grabbable = GetComponent<OVRGrabbable>();
        trajectory = GetComponent<Trajectory>();
        Throw = (ThrowMode)ThrowModeUI.CurrentMode;
        Debug.Log(Throw);

        //NOTE:ボールをつかんだ瞬間ストックを行うストリーム
        this.UpdateAsObservable()
            .Where(_ => grabbable.isGrabbed && !lastIsGrabbed)
            .Subscribe(_ =>
            {
                gameObject.transform.parent = null;
                trajectory.Retry();
            });

        //NOTE:投げられたことを検知して軌跡の生成を始めるストリーム
        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                if (!grabbable.isGrabbed && lastIsGrabbed)
                {
                    startThrowing();
                }
                lastIsGrabbed = grabbable.isGrabbed;
            });

        //NOTE:軌跡生成ストリーム
        this.UpdateAsObservable()
            .Where(_ => doThrow)
            .Subscribe(_ => 
            {
                throwBall();
                elapsedTime += Time.deltaTime;
            });

        //NOTE:投げられたボールを削除してリスポンさせるストリーム
        this.UpdateAsObservable()
            .Where(_ => doThrow)
            .Where(_ => gameObject.transform.position.y < 0)
            .Subscribe(_ =>
            {
                respown();
                Destroy(gameObject);
            });

        var rigidBody = GetComponent<Rigidbody>();

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {

                lastVelocitis.Add(rigidBody.velocity);

                if (lastVelocitis.Count > velocityLength)
                {
                    lastVelocitis.RemoveAt(0);
                }
            });
    }

    void startThrowing()
    {
        throwVelocity = lastVelocitis[0];
        throwPosition = gameObject.transform.position;
        trajectoryParent = trajectory.CreateParent(throwPosition);
        doThrow = true;
    }

    void throwBall()
    {
        float coordinateX = 0;
        float coordinateY = 0;
        float coordinateZ = 0;

        switch (Throw)
        {
            case ThrowMode.Normal:
                coordinateX = throwPosition.x + throwVelocity.x * elapsedTime;
                coordinateY = throwPosition.y + throwVelocity.y * elapsedTime - GRAVITY * Mathf.Pow(elapsedTime, 2) / 2;
                coordinateZ = throwPosition.z + throwVelocity.z * elapsedTime;
                break;
            case ThrowMode.Horizontal:
                coordinateX = throwPosition.x + throwVelocity.x * elapsedTime;
                coordinateY = throwPosition.y - GRAVITY * Mathf.Pow(elapsedTime, 2) / 2;
                coordinateZ = throwPosition.z + throwVelocity.z * elapsedTime;
                break;
            case ThrowMode.Vartical:
                coordinateX = throwPosition.x;
                coordinateY = throwPosition.y + throwVelocity.y * elapsedTime - GRAVITY * Mathf.Pow(elapsedTime, 2) / 2;
                coordinateZ = throwPosition.z;
                break;
        }



        var position = new Vector3(coordinateX, coordinateY, coordinateZ);

        trajectory.CreateTrajectory(position, elapsedTime);
        gameObject.transform.position = position;
    }

    void respown()
    {
        var instantBall = Instantiate(ball, Vector3.zero, Quaternion.identity);
        instantBall.name = ball.name;
        var trajectoryComponent = instantBall.GetComponent<Trajectory>();

        switch (Throw)
        {
            case ThrowMode.Normal:
                break;
            case ThrowMode.Horizontal:
                throwVelocity = new Vector3(throwVelocity.x, 0, throwVelocity.z);
                break;
            case ThrowMode.Vartical:
                throwVelocity = new Vector3(0, throwVelocity.y, 0);
                break;
        }


        trajectoryComponent.LastTrajectoryData = Tuple.Create(trajectoryParent, throwVelocity);

        StockTrajectoryUI.Trajectory = trajectoryComponent;

        var objectUI = FindObjectOfType<ObjectUI>();
        instantBall.transform.parent = objectUI.transform;
        instantBall.transform.localPosition = new Vector3(0, 0.4f, -0.1f);
    }
}
