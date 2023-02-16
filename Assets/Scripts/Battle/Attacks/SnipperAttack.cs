using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipperAttack : EnemyAttackPatternBase
{
    public GameObject bullet;
    public int bulletNum;
    public float bulletAngle;
    public float bulletSpeed;
    public int bulletWaves;
    public float bulletFrequencePerShoot;
    public float firstShootDelay;

    private Transform player;
    private float lastFire;

    public override void ParamInit()
    {
        base.ParamInit();
        lastFire = Time.fixedTime + firstShootDelay;
        player = GameController.GetInstance().player.transform;
    }

    public override void Fire()
    {
        if (player == null)
        {
            player = GameController.GetInstance().player.transform;
            if(player==null)
                return;
        }

        if (Time.fixedTime - lastFire >= fireInterval)
        {
            StartCoroutine(OnceShoot());
            lastFire = Time.fixedTime;
        }
    }

    IEnumerator OnceShoot()
    {
        int shootTime = 0;
        float lastShoot = Time.fixedTime - bulletFrequencePerShoot;

        while (true)
        {
            if (Time.fixedTime - lastShoot >= bulletFrequencePerShoot)
            {
                float targetDegree = Vector2.SignedAngle(player.position - transform.position, Vector2.up);
                for (int i = -bulletNum / 2; i * 2 <= bulletNum - 1; i++)
                {
                    GameObject tmpBullet = Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, 0), LevelManager.bulletContainer);
                    BulletBase bulletScript = tmpBullet.GetComponent<BulletBase>();
                    bulletScript.Degree = (i + (bulletNum % 2 == 0 ? 0.5f : 0)) * bulletAngle + targetDegree;
                    bulletScript.Damage = damage;
                    bulletScript.Distance = bulletDistance;
                    bulletScript.Speed = bulletSpeed;
                }
                lastShoot += bulletFrequencePerShoot;
                shootTime++;
                if (shootTime >= bulletWaves)
                    yield break;
            }
            yield return null;
        }
    }
}
