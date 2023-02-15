using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phantom : DamageTaker
{
    protected override void Awake()
    {
        base.Awake();
        destroyOnDeath = false;
    }

    protected override void CheckDeath()
    {
        base.CheckDeath();

        if(dead)
        {
            GameController.Instance.ChangeToPhantom(this);
        }
    }
}
