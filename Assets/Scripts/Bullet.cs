using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public Vector2 velocity;

    public delegate void OnTimer(Bullet self);
    public delegate void OnHitObject(Bullet self, Collider2D collision);
    public OnHitObject onHit;

    public bool destroyOnHit = true;
    public float lifespan = 5.0f;

    public IObjectPool<Bullet> bulletPool;

    //void OnEnable()
    //{
    //    Invoke("Release", lifespan);
    //}

    // Update is called once per frame
    void Update()
    {
        transform.Translate(velocity * Time.deltaTime);
        lifespan -= Time.deltaTime;
        if(lifespan < 0)
        {
            Release();
        }
    }

    void Release()
    {
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
