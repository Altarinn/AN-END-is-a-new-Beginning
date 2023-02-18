using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public Vector2 velocity;
    public SpriteRenderer spriteRenderer;

    public delegate void OnTimer(Bullet self);
    public delegate void OnUpdateBullet(Bullet self);
    public delegate void OnHitObject(Bullet self, Collider2D collision);
    
    public OnHitObject onHit;
    public OnTimer onRelease;
    public OnUpdateBullet onUpdateBullet = UpdateBulletLinear;

    public bool destroyOnHit = true;
    public bool sticky = false; // If sticky, will stick to and move together with enemy after hit.
    public float lifespan = 5.0f, damage = 1.0f;

    public IObjectPool<Bullet> bulletPool;

    static int enemyLayer, playerLayer;

    private void Awake()
    {
        if(spriteRenderer == null) { spriteRenderer = GetComponent<SpriteRenderer>(); }

        enemyLayer = LayerMask.NameToLayer("EnemyBullet");
        playerLayer = LayerMask.NameToLayer("PlayerBullet");
    }

    private void OnDisable()
    {
        // Re-initialize myself
        onHit = null;
        onRelease = null;
        onUpdateBullet = Bullet.UpdateBulletLinear;
        damage = 1.0f;
        sticky = false;
        destroyOnHit = false;
        lifespan = 5.0f;
        StopAllCoroutines();
    }

    //void OnEnable()
    //{
    //    Invoke("Release", lifespan);
    //}

    // Update is called once per frame
    void Update()
    {
        onUpdateBullet?.Invoke(this);

        lifespan -= Time.deltaTime;
        if(lifespan < 0)
        {
            Release();
        }
    }

    public void ApplySetting(BulletSetting setting)
    {
        if (spriteRenderer == null) { spriteRenderer = GetComponent<SpriteRenderer>(); }

        spriteRenderer.sprite = setting.sprite;
        spriteRenderer.color = setting.color;
        destroyOnHit = !setting.dontDestroyOnHit;
        lifespan = 5.0f + setting.extraLifeSpan;

        if(setting.isEnemy)
        {
            gameObject.layer = enemyLayer;
        }
        else
        {
            gameObject.layer = playerLayer;
        }
    }

    public static void UpdateBulletLinear(Bullet self)
    {
        self.transform.Translate(self.velocity * Time.deltaTime);
    }

    public void Release()
    {
        onRelease?.Invoke(this);
        bulletPool.Release(this);
    }

    //protected void OnColliderEnter2D(Collision2D collision)
    //{
    //    if (this.gameObject.activeSelf == false) { return; }

    //    if (onHit != null) { onHit(this, collision); }

    //    if (destroyOnHit) { Release(); }
    //}

    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if(this.gameObject.activeSelf == false) { return; }

        if (onHit != null) { onHit(this, collider); }

        var dt = collider.GetComponent<DamageTaker>();
        if (dt != null)
        {
            dt.Damage(damage, velocity.normalized);
            if (sticky) { transform.parent = collider.transform; }
        }

        if (destroyOnHit) { Release(); }
    }

    IEnumerator DelayedCoroutine(float sec, OnTimer foo)
    {
        yield return new WaitForSeconds(sec);
        foo(this);
        yield break;
    }

    public void DelayedInvoke(float sec, OnTimer foo)
    {
        StartCoroutine(DelayedCoroutine(sec, foo));
    }
}
