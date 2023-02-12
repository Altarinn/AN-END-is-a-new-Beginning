using UnityEngine;

public class CurveBullet : BulletBase
{
    [Tooltip("���ٶ�")]
    public float speedChangeValue = 0;
    [Tooltip("�ٶȱ仯���� (ǧ�ֱ�)")]
    public float speedChangeRatio = 1000;
    [Tooltip("�Ƕȱ任�ٶ�")]
    public float degreeChangeValue = 0;

    private float movementX;
    private float movementY;

    protected override void ParamInit()
    {
        base.ParamInit();
        speedChangeRatio /= 1000;
    }

    override protected void MoveMethod()
    {
        Speed = Speed * Mathf.Pow(speedChangeRatio, Time.deltaTime / 1) + speedChangeValue * Time.deltaTime;
        Degree = Degree + degreeChangeValue * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, 0, -Degree);
        movementX = Speed * Mathf.Sin(Degree / 180f * Mathf.PI) * Time.deltaTime;
        movementY = Speed * Mathf.Cos(Degree / 180f * Mathf.PI) * Time.deltaTime;
        transform.Translate(new Vector2(movementX, movementY), Space.World);
    }
}