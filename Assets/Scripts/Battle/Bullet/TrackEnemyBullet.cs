using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackEnemyBullet : BulletBase
{
    [Label("加速度")]
    public float speedChangeValue = 0;
    [Label("速度变化倍率")]
    [Tooltip("填写千分比")]
    public float speedChangeRatio = 1000;
    [Label("追踪角速度")]
    [Tooltip("速度越大，追踪能力越强")]
    public float degreeSpeed = 5;
    [Label("追踪距离")]
    [Tooltip("只能检索到距离内的敌人")]
    public float trackRange = 20;

    private Transform target;
    private float movementX;
    private float movementY;
    private float distanceNow = 0;
    private float lastCheckTarget;
    private float checkFrequence = 0.5f;

    private float minDegree = 0.2f;

    public override void CheckPos()
    {
        if (distanceNow >= Distance)
        {
            Destroy(this.gameObject);
        }
    }

    protected override void ParamInit()
    {
        base.ParamInit();
        speedChangeRatio /= 1000;
        lastCheckTarget = Time.fixedTime;
    }

    protected override void MoveMethod()
    {
        if (Time.fixedTime - lastCheckTarget >= checkFrequence || target == null)
        {
            target = UsualTools.FindNearestEnemy(transform, trackRange);
        }

        Speed = Speed * Mathf.Pow(speedChangeRatio, Time.deltaTime) + speedChangeValue * Time.deltaTime;
        float changeDegree;
        if (target != null)
            changeDegree = Vector2.SignedAngle(Vector2.up, target.position - transform.position);
        else
            changeDegree = 0;
        if (Mathf.Abs(changeDegree - Degree) > minDegree)
            Degree -= Mathf.Sign(changeDegree) * degreeSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, -Degree);
        movementX = Speed * Mathf.Sin(Degree / 180f * Mathf.PI) * Time.deltaTime;
        movementY = Speed * Mathf.Cos(Degree / 180f * Mathf.PI) * Time.deltaTime;
        transform.Translate(new Vector2(movementX, movementY), Space.World);
        distanceNow += Mathf.Sqrt(movementX * movementX + movementY * movementY);
    }
}