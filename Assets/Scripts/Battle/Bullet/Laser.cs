using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : BulletBase
{
    [Label("持续时间")]
    public float constantDuration;

    private float instantiateTime;

    protected override void ParamInit()
    {
        base.ParamInit();
        instantiateTime = Time.fixedTime;
        transform.rotation = Quaternion.Euler(0, 0, -Degree);
    }

    /*protected override void OnHit(Collider2D collision)
    {
        BuffManager tmpBuffManager = collision.gameObject.GetComponent<BuffManager>();
        if (tmpBuffManager == null)
            return;
        tmpBuffManager.AddBuffToQueue(Buffs, unit);
    }*/

    public void EffectOn()
    {
        GetComponent<Collider2D>().enabled = true;
        animator.SetTrigger("InitFinish");
    }

    protected override void MoveMethod()
    {
        //transform.position = unit.transform.position;
    }

    public override void CheckPos()
    {
        if (Time.fixedTime - instantiateTime >= constantDuration)
            OnSmash();
    }
}
