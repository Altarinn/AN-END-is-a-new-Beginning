using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravityBomb : AttackPatternBase
{
    public BulletSetting setting;

    public float flySpeed = 18, explosionSpeed = 16;
    public int explosionNum = 36;
    public float gravity = 2.0f, maxFall = 20.0f;
    public AudioClip bomb_l;
    public AudioClip bomb_s;
    AudioSource m_MyAudioSource;

    float initialOffset = 0.4f;

    BulletManager b;

    private void Start()
    {
        b = BulletManager.Instance;
        m_MyAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    //void Update()
    //{
    //}

    public override void Fire(Vector2 origin, Vector2 direction)
    {
        m_MyAudioSource?.PlayOneShot(bomb_l, 1.0f);
        b.BulletFanShots(setting, explosionNum, transform.position, direction * explosionSpeed, 360, initialOffset)
        .OnUpdate((self) =>
        {
            self.velocity.y = Mathf.Max(-maxFall, self.velocity.y - gravity * Time.deltaTime);
            self.transform.Translate(self.velocity * Time.deltaTime);
        })
        .OnHit((self, coll) =>
        {
            m_MyAudioSource?.PlayOneShot(bomb_s, 0.6f);
            b.BulletFanShots(setting, 12, self.transform.position, Vector3.up * explosionSpeed, 360, initialOffset)
            .SetLife(0.05f);
        });
    }
}
