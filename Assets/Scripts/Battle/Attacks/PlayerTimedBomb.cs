using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTimedBomb : AttackPatternBase
{
    public BulletSetting setting;
    BulletSetting setting2;

    public float flySpeed = 18, explosionSpeed = 16;
    public int explosionNum = 36;
    public float gravity = 2.0f, maxFall = 20.0f;
    public float delay = 5.0f;
    public AudioClip mega;
    public AudioClip bomb_m;
    public AudioClip bomb_s;
    public AudioClip slime;
    AudioSource m_MyAudioSource;

    float initialOffset = 0.4f;

    BulletManager b;

    private void Start()
    {
        b = BulletManager.Instance;

        m_MyAudioSource = GetComponent<AudioSource>();
        setting2 = setting;
    }

    // Update is called once per frame
    //void Update()
    //{
    //}

    public override void Fire(Vector2 origin, Vector2 direction)
    {
        m_MyAudioSource.PlayOneShot(mega, 1.0f);
        b.BulletFanShots(setting, 1, origin, (direction + Vector2.up / 1f).normalized * flySpeed, 60, initialOffset)
        .OnUpdate((self) => 
        { 
            self.velocity.y = Mathf.Max(-maxFall, self.velocity.y - gravity * Time.deltaTime);
            self.transform.Translate(self.velocity * Time.deltaTime);
        })
        .OnHit((self, coll) =>
        {
            //Vector2 collisionNormal = (Vector2)self.transform.position - coll.ClosestPoint(self.transform.position);
            //collisionNormal.Normalize();
            m_MyAudioSource.PlayOneShot(slime, 0.6f);
            b.BulletFanShots(setting, 1, self.transform.position, Vector2.zero, 360, 0.0f)
            .SetDestroyOnHit(false)
            .SetLife(delay)
            .OnRelease((self) => {
                m_MyAudioSource.PlayOneShot(bomb_m, 0.9f);
                b.BulletFanShots(setting, explosionNum, self.transform.position, Vector3.up * explosionSpeed, 360, initialOffset)
                .OnUpdate((self) =>
                {
                    self.velocity.y = Mathf.Max(-maxFall, self.velocity.y - gravity * Time.deltaTime);
                    self.transform.Translate(self.velocity * Time.deltaTime);
                })
                .OnRelease((self) =>
                {
                    m_MyAudioSource.PlayOneShot(bomb_s, 0.5f);
                    b.BulletFanShots(setting, 12, self.transform.position, Vector3.up * explosionSpeed, 360, 0.1f)
                    .SetLife(0.1f);
                });
            });

            b.BulletFanShots(setting, 1, self.transform.position, Vector2.zero, 360, 0.0f)
            .SetDestroyOnHit(false)
            .SetLife(delay * 2)
            .OnRelease((self) => {
                m_MyAudioSource.PlayOneShot(bomb_m, 0.9f);
                b.BulletFanShots(setting, explosionNum / 2, self.transform.position, Vector3.up * explosionSpeed * 0.667f, 360, initialOffset)
                .OnUpdate((self) =>
                {
                    self.velocity.y = Mathf.Max(-maxFall, self.velocity.y - gravity * Time.deltaTime);
                    self.transform.Translate(self.velocity * Time.deltaTime);
                })
                .OnHit((self, coll) =>
                {
                    m_MyAudioSource.PlayOneShot(bomb_s, 0.5f);
                    b.BulletFanShots(setting, 12, self.transform.position, Vector3.up * explosionSpeed, 360, 0.1f)
                    .SetLife(0.1f);
                });
            });

            b.BulletFanShots(setting, 1, self.transform.position, Vector2.zero, 360, 0.0f)
            .SetDestroyOnHit(false)
            .SetLife(delay * 3)
            .OnRelease((self) => {
                m_MyAudioSource.PlayOneShot(bomb_m, 0.9f);
                b.BulletFanShots(setting, explosionNum / 4, self.transform.position, Vector3.up * explosionSpeed * 0.333f, 360, initialOffset)
                .OnUpdate((self) =>
                {
                    self.velocity.y = Mathf.Max(-maxFall, self.velocity.y - gravity * Time.deltaTime);
                    self.transform.Translate(self.velocity * Time.deltaTime);
                })
                .OnHit((self, coll) =>
                {
                    m_MyAudioSource.PlayOneShot(bomb_s, 0.5f);
                    b.BulletFanShots(setting, 12, self.transform.position, Vector3.up * explosionSpeed, 360, 0.1f)
                    .SetLife(0.1f);
                });
            });
        });
    }

}
 