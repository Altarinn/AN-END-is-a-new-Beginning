using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : PlayerInputHandler
{
    public Bullet bulletPrefab;

    protected override void HandleInput(FrameInput input)
    {
        if(input.PrimaryFire)
        {
            Fire(transform.position);
        }
    }

    void Fire(Vector2 pos)
    {
        //SpriteRenderer test = Instantiate(bulletPrefab, new Vector3(pos.x, pos.y, -2.0f), Quaternion.identity).GetComponent<SpriteRenderer>();
        //test.color = Random.ColorHSV();

        BulletManager.Instance
            .BulletFanShots(bulletPrefab, 3, pos, Vector3.right * 20, 60, 0.5f)
            .OnHit((self, coll) =>
            {
                BulletManager.Instance
                    .BulletFanShots(bulletPrefab, 25, self.transform.position, Vector3.up * 20, 360, 0.5f)
                    .OnTimer(1, (self) => BulletManager.Instance.BulletFanShots(bulletPrefab, 3, self.transform.position, Vector3.up * 20, 360, 0.5f));
            })
            .OnTimer(0.1f, (self) => BulletManager.Instance.BulletFanShots(bulletPrefab, 3, self.transform.position, Vector3.up * 20, 360, 0.5f));
    }
}
