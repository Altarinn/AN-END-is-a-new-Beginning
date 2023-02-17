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

    float initialOffset = 0.3f;

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
            // Try these patterns
            //b.BulletZigzagShots(setting, 1, origin, flySpeed, 0.05f, 45.0f, 0f, 0.5f)
            //b.BulletCurveShots(setting, 1, origin, flySpeed, 0f, 0.05f, 10f, 0.5f)
            //b.BulletFocusedShots(setting, 3, origin, direction * flySpeed, 0.08f, 10f)
            b.BulletShot(setting, origin, direction * flySpeed)
            .OnHit((self, coll) =>
            {
                b.BulletFanShots(setting, explosionNum, self.transform.position, direction * explosionSpeed, 300, initialOffset);
            });
        }

        currentBurstCount++;
    }
}
