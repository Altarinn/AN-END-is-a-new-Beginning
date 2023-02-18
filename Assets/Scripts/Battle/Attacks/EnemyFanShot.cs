using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFanShot : AttackPatternBase
{
    public BulletSetting setting;

    public float flySpeed = 18, shotDegree = 120;
    public int shotNum = 7;

    float initialOffset = 0.4f;

    BulletManager b;

    private void Start()
    {
        b = BulletManager.Instance;
    }

    public override void Fire(Vector2 origin, Vector2 direction)
    {
        b.BulletFanShots(setting, shotNum, origin, (direction).normalized * flySpeed, shotDegree, initialOffset);
    }
}
