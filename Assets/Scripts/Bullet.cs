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
    public float lifespan = 5.0f;

    public IObjectPool<Bullet> bulletPool;

    private void Awake()
    {
        if(spriteRenderer == null) { spriteRenderer = GetComponent<SpriteRenderer>(); }
    }

    private void OnDisable()
    {
        // Re-initialize myself
        onHit = null;
        onRelease = null;
        onUpdateBullet = Bullet.UpdateBulletLinear;
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
