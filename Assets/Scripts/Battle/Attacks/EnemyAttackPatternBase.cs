using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackPatternBase : MonoBehaviour
{
    public float fireInterval;
    public float bulletDistance;
    public float damage;

    public virtual void Fire() { }

    private void Start()
    {
        ParamInit();
    }

    public void Update()
    {
        Fire();
    }

    public virtual void ParamInit()
    {

    }
}
