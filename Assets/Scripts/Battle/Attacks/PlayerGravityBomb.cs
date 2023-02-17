using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravityBomb : AttackPatternBase
{
    public BulletSetting setting;

    public float flySpeed = 18, explosionSpeed = 16;
    public int explosionNum = 36;
    public float gravity = 2.0f, maxFall = 20.0f;

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
        b.BulletFanShots(setting, 1, origin, (direction + Vector2.up / 1.732f).normalized * flySpeed, 60, initialOffset)
        .OnUpdate((self) => 
        { 
            self.velocity.y = Mathf.Max(-maxFall, self.velocity.y - gravity * Time.deltaTime);
            self.transform.Translate(self.velocity * Time.deltaTime);
        })
        .OnHit((self, coll) =>
        {
            b.BulletFanShots(setting, explosionNum, self.transform.position, Vector3.up * explosionSpeed, 360, initialOffset)
            .OnUpdate((self) =>
            {
                self.velocity.y = Mathf.Max(-maxFall, self.velocity.y - gravity * Time.deltaTime);
                self.transform.Translate(self.velocity * Time.deltaTime);
            })
            .OnHit((self, coll) =>
            {
                b.BulletFanShots(setting, 12, self.transform.position, Vector3.up * explosionSpeed, 360, initialOffset)
                .SetLife(0.05f);
            });
        });
    }
}
