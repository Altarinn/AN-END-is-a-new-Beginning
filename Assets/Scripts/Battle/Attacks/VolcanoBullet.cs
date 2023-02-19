using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoBullet : AttackPatternBase
{
    public BulletSetting setting;

    public float flySpeed = 18, explosionSpeed = 30;
    public int explosionNum = 36;
    public float explosionSpeedYOffset = 1.0f;
    public float gravity = 5.0f, maxFall = 30.0f;
    public float explosionRange = 0.1f;
    //make the explosion a circle(x
    public float explosionOffset = 0.4f;

    float initialOffset = 0.4f;

    BulletManager b;

    private void Start()
    {
        b = BulletManager.Instance;
    }

    // Update is called once per frame
    //void Update()
    //{
    //}

    public override void Fire(Vector2 origin, Vector2 direction)
    {
        b.BulletFanShots(setting, 1, origin, (direction + Vector2.up * explosionSpeedYOffset).normalized * flySpeed, 60, initialOffset)
        .OnUpdate((self) =>
        {
            self.velocity.y = Mathf.Max(-maxFall, self.velocity.y - gravity * Time.deltaTime);
            self.transform.Translate(self.velocity * Time.deltaTime);
        })
        .OnHit((self, coll) =>
        {
            b.BulletFanShots(setting, explosionNum, self.transform.position + Vector3.up * explosionOffset, Vector3.up * explosionSpeed, 360, initialOffset).SetLife(explosionRange)
            .OnUpdate((self) =>
            {
                self.transform.Translate(self.velocity * Time.deltaTime);
            });
        });
    }
}
