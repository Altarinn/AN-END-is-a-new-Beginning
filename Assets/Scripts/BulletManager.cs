using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public struct BulletSetting
{
    public Sprite sprite;
    public Color color;
}

public class BulletManager : SingletonMonoBehaviour<BulletManager>
{
    public Bullet bulletPrefab;
    public Transform bulletContainer;

    #region BulletPool

    // We use an object pool to manage all bullets to avoid create / destroy them frequently.

    public IObjectPool<Bullet> bulletPool;
    int nBulletsActive = 0;

    Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab.gameObject, bulletContainer).GetComponent<Bullet>();
        bullet.bulletPool = bulletPool;

        return bullet;
    }

    void InitializePool()
    {
        bulletPool = new ObjectPool<Bullet>(
            CreateBullet,
            (bullet) => { nBulletsActive++;  bullet.gameObject.SetActive(true); },
            (bullet) => { nBulletsActive--;  bullet.gameObject.SetActive(false); bullet.onHit = null; bullet.StopAllCoroutines(); },
            (bullet) => { nBulletsActive--;  Destroy(bullet.gameObject); Debug.LogWarning("Please increase bullet pool size."); },
            true,
            100,
            10000
        );
    }

    //GUIStyle style;
    //bool _uiInitialized = false;

    private void OnGUI()
    {
        GUI.skin = DebugUI.Instance.debugUISkin;

        //if(!_uiInitialized)
        //{
        //    style = new GUIStyle();
        //    style.normal = new GUIStyleState();
        //    style.normal.textColor = Color.black;
        //    style.normal.background = Texture2D.whiteTexture;
        //}
        GUILayout.Label(
            $"Pool act: {nBulletsActive}\nInactive: {bulletPool.CountInactive}\nCapacity: {nBulletsActive + bulletPool.CountInactive}");
    }

    #endregion

    public class BulletList
    {
        Bullet[] bullets;

        public BulletList(Bullet bullet)
        {
            bullets = new Bullet[1] {bullet};
        }

        public BulletList(Bullet[] bullets)
        {
            this.bullets = bullets;
        }

        /// <summary>
        /// Action after hits an object.
        /// </summary>
        /// <param name="onHitFn">public delegate void OnHitObject(Bullet self, Collider2D collision)</param>
        /// <returns></returns>
        public BulletList OnHit(Bullet.OnHitObject onHitFn)
        {
            foreach (var bullet in bullets)
            {
                bullet.onHit = onHitFn;
            }

            return this;
        }

        /// <summary>
        /// Action after a given time after bullet has spawned.
        /// </summary>
        /// <param name="sec">Time in seconds.</param>
        /// <param name="action">public delegate void OnTimer(Bullet self)</param>
        /// <returns></returns>
        public BulletList OnTimer(float sec, Bullet.OnTimer action)
        {
            foreach (var bullet in bullets)
            {
                bullet.DelayedInvoke(sec, action);
            }

            return this;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        GameObject obj = GameObject.Find("BulletsContainer");
        if(obj == null)
        {
            obj = new GameObject("TempBulletContainer");
        }
        bulletContainer = obj.transform;

        InitializePool();
    }

    public Bullet InitializeSettings(BulletSetting settings)
    {
        Debug.LogWarning("DEBUG ONLY FEATURE, Please don't call this function in production");

        GameObject obj = new GameObject("bullet");
        var sr = obj.AddComponent<SpriteRenderer>();

        sr.sprite = settings.sprite;
        sr.color = settings.color;

        // TODO: Colliders

        return obj.AddComponent<Bullet>();
    }

    public BulletList BulletShot(BulletSetting settings, Vector2 position, Vector2 velocity)
    {
        Bullet bullet = bulletPool.Get();

        // TODO: lifespan, sprite, etc.

        bullet.transform.position = position;
        bullet.velocity = velocity;
        bullet.lifespan = 5.0f;

        return new BulletList(bullet);
    }

    /// <summary>
    /// Shots N bullets from (-degree/2, ..., degree/2) with equal intervals.
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="N"></param>
    /// <param name="position"></param>
    /// <param name="centerVelocity"></param>
    /// <param name="degree"></param>
    /// <param name="initialDistance"></param>
    /// <returns></returns>
    public BulletList BulletFanShots(Bullet prefab, int N, Vector2 position, Vector2 centerVelocity, float degree, float initialDistance = 0.0f)
    {
        Bullet[] bullets = new Bullet[N];

        for(int i = 0; i < N; i++)
        {
            Vector2 velocity = centerVelocity;
            if (N > 1)
            {
                velocity = Quaternion.AngleAxis(degree / (N - 1) * i - degree / 2, Vector3.forward) * centerVelocity;
            }

            Bullet bullet = bulletPool.Get();

            // TODO: lifespan, sprite, etc.

            bullet.transform.position = position + velocity.normalized * initialDistance;
            bullet.velocity = velocity;
            bullet.lifespan = 5.0f;

            bullets[i] = bullet;
        }

        return new BulletList(bullets);
    }
}
