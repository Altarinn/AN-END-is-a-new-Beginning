using UnityEngine;

public class LinearBullet : BulletBase
{
    private float speed;
    private float posNow = 0;
    private Vector2 targetPos;

    protected override void ParamInit()
    {
        base.ParamInit();
        startPos = transform.position;
        targetPos = transform.position + new Vector3(Mathf.Sin(Degree / 180f * Mathf.PI), Mathf.Cos(Degree / 180f * Mathf.PI)) * Distance;
        transform.rotation = Quaternion.Euler(0, 0, -Degree);
        speed = Speed / Distance;
    }

    protected override void MoveMethod()
    {
        posNow += speed * Time.deltaTime;
        transform.position = (targetPos - startPos) * posNow + startPos;
    }
}
