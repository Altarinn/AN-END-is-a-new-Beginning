using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMegaman : AttackPatternBase
{
    public BulletSetting setting;

    public float flySpeed = 18, explosionSpeed = 10;

    public int burstNum = 3, explosionNum = 16;
    public float burstRecoverSec = 0.35f;

    int currentBurstCount = 0;
    float burstTimer = 0;

    BulletManager b;

    private void Start()
    {
        b = BulletManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (burstTimer > burstRecoverSec)
        {
            burstTimer = 0;
            currentBurstCount = 0;
        }

        if (currentBurstCount >= burstNum)
        {
            burstTimer += Time.deltaTime;
        }
    }

    public override void Fire(Vector2 origin, Vector2 direction)
    {
        if(currentBurstCount < burstNum)
        {
            b.BulletFanShots(setting, 1, origin, direction * flySpeed, 60, 0.5f)
            .OnHit((self, coll) =>
            {
                b.BulletFanShots(setting, explosionNum, self.transform.position, Vector3.up * explosionSpeed, 360, 0.5f);
            });
        }

        currentBurstCount++;
    }
}
