using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System.Linq;

[System.Serializable]
public struct BulletSetting
{
    public Sprite sprite;
    public Color color;

    public bool dontDestroyOnHit;

    [Tooltip("lifespan = 5.0 + extra sec")]
    public float extraLifeSpan;
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
            (bullet) => 
            { 
                nBulletsActive--;
                bullet.gameObject.SetActive(false);
            },
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

        public BulletList OnRelease(Bullet.OnTimer action)
        {
            foreach (var bullet in bullets)
            {
                bullet.onRelease = action;
            }

            return this;
        }

        public BulletList OnUpdate(Bullet.OnUpdateBullet action)
        {
            foreach (var bullet in bullets)
            {
                bullet.onUpdateBullet = action;
            }

            return this;
        }

        public BulletList SetDestroyOnHit(bool value)
        {
            foreach (var bullet in bullets)
            {
                bullet.destroyOnHit = value;
            }

            return this;
        }

        public BulletList SetLife(float value)
        {
            foreach (var bullet in bullets)
            {
                bullet.lifespan = value;
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
    public BulletList BulletFanShots(BulletSetting settings, int N, Vector2 position, Vector2 centerVelocity, float degree, float initialDistance = 0.0f)
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
            bullet.ApplySetting(settings);

            bullet.transform.position = position + velocity.normalized * initialDistance;
            bullet.velocity = velocity;
            bullet.lifespan = 5.0f;

            bullets[i] = bullet;
        }

        return new BulletList(bullets);
    }

    /// <summary> 
    /// Shots N bullets from the same position with a slight angle difference and a perpendicular offset. 
    /// </summary> 
    /// <param name=“settings”></param> 
    /// <param name=“N”></param> 
    /// <param name=“position”></param> 
    /// <param name=“centerVelocity”></param> 
    /// <param name=“bulletDistance”></param> 
    /// <param name=“targetDegree”></param> 
    /// <returns></returns>
    public BulletList BulletFocusedShots(BulletSetting settings, int N, Vector2 position, Vector2 centerVelocity, float bulletDistance = 0.5f, float targetDegree = 1.0f)
    {
        Bullet[] bullets = new Bullet[N];

        for(int i = 0; i < N; i++)
        {
            Vector2 velocity = centerVelocity;
            if (N > 1)
            {
                velocity = Quaternion.AngleAxis(-targetDegree * i, Vector3.forward) * centerVelocity;
            }

            Bullet bullet = bulletPool.Get();

            bullet.ApplySetting(settings);

            bullet.transform.position = position + Vector2.Perpendicular(centerVelocity) * bulletDistance * i;
            bullet.velocity = velocity;
            bullet.lifespan = 5.0f;

            bullets[i] = bullet;
        }

        return new BulletList(bullets);
    }

    /// <summary> 
    /// Shots N bullets from the same position with a spiral pattern. 
    /// </summary> 
    /// <param name=“settings”></param> 
    /// <param name=“N”></param> 
    /// <param name=“position”></param> 
    /// <param name=“speed”></param>
    /// <param name=“angle”></param> 
    /// <param name=“angleIncrement”></param> 
    /// <param name=“initialDistance”></param> 
    /// <returns></returns>
    public BulletList BulletSpiralShots(BulletSetting settings, int N, Vector2 position, float speed, float angle, float angleIncrement, float initialDistance = 0.0f)
    {
        Bullet[] bullets = new Bullet[N];

        for(int i = 0; i < N; i++)
        {
            // Calculate the velocity based on the angle and speed
            Vector2 velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * speed;

            Bullet bullet = bulletPool.Get();

            // TODO: lifespan, sprite, etc.
            bullet.ApplySetting(settings);

            // Calculate the initial position based on the angle and distance
            bullet.transform.position = position + velocity.normalized * initialDistance;
            bullet.velocity = velocity;
            bullet.lifespan = 5.0f;

            bullets[i] = bullet;

            // Increment the angle for the next bullet
            angle += angleIncrement;
        }

        return new BulletList(bullets);
    }

    /// <summary> 
    /// Shots N bullets from the same position with a circular pattern and a periodic velocity change. 
    /// </summary> 
    /// <param name=“settings”></param> 
    /// <param name=“N”></param> 
    /// <param name=“position”></param> 
    /// <param name=“speed”></param> 
    /// <param name=“initialAngle”></param> 
    /// <param name=“interval”></param> 
    /// <param name=“angleIncrement”></param> 
    /// <param name=“initialDistance”></param> 
    /// <returns></returns>
    public BulletList BulletCurveShots(BulletSetting settings, int N, Vector2 position, float speed, float initialAngle = 0.0f, float interval = 0.05f, float angleIncrement = 10f, float initialDistance = 0.0f)
    {
        Bullet[] bullets = new Bullet[N];

        for(int i = 0; i < N; i++)
        {
            // Calculate the angle based on the number of bullets and the initial angle
            float angle = initialAngle + 360.0f / N * i;

            // Calculate the velocity based on the angle and speed
            Vector2 velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * speed;

            Bullet bullet = bulletPool.Get();

            // TODO: lifespan, sprite, etc.
            bullet.ApplySetting(settings);

            // Calculate the initial position based on the angle and distance
            bullet.transform.position = position + velocity.normalized * initialDistance;
            bullet.velocity = velocity;
            bullet.lifespan = 5.0f;

            // Add a coroutine to change the velocity periodically
            bullet.StartCoroutine(CurvePattern(bullet, angle, interval, angleIncrement));

            bullets[i] = bullet;
        }

        return new BulletList(bullets);
    }

    // A coroutine to change the velocity of the bullet to form a star shape
    private IEnumerator CurvePattern(Bullet bullet, float initialAngle, float interval, float angleIncrement)
    {
        // The current angle of the bullet
        float angle = initialAngle;

        // The speed of the bullet
        float speed = bullet.velocity.magnitude;

        // Repeat until the bullet is destroyed
        while (bullet != null)
        {
            // Wait for the interval
            yield return new WaitForSeconds(interval);

            // Increment the angle
            angle += angleIncrement;

            // Calculate the new velocity based on the angle and speed
            Vector2 velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * speed;

            // Set the new velocity to the bullet
            bullet.velocity = velocity;
        }
    }

    /// <summary> 
    /// Shots N bullets from the same position with a circular pattern and a zigzag velocity change. 
    /// </summary> 
    /// <param name=“settings”></param> 
    /// <param name=“N”></param> 
    /// <param name=“position”></param> 
    /// <param name=“speed”></param> 
    /// <param name=“interval”></param> 
    /// <param name=“angleIncrement”></param> 
    /// <param name=“initialAngle”></param> 
    /// <param name=“initialDistance”></param> 
    /// <returns></returns>
    public BulletList BulletZigzagShots(BulletSetting settings, int N, Vector2 position, float speed, float interval, float angleIncrement, float initialAngle = 0.0f, float initialDistance = 0.0f)
    {
        Bullet[] bullets = new Bullet[N];

        for(int i = 0; i < N; i++)
        {
            // Calculate the angle based on the number of bullets and the initial angle
            float angle = initialAngle + 360.0f / N * i;

            // Calculate the velocity based on the angle and speed
            Vector2 velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * speed;

            Bullet bullet = bulletPool.Get();

            // TODO: lifespan, sprite, etc.
            bullet.ApplySetting(settings);

            // Calculate the initial position based on the angle and distance
            bullet.transform.position = position + velocity.normalized * initialDistance;
            bullet.velocity = velocity;
            bullet.lifespan = 5.0f;

            // Add a coroutine to change the velocity periodically
            bullet.StartCoroutine(ZigzagPattern(bullet, angle, interval, angleIncrement));

            bullets[i] = bullet;
        }

        return new BulletList(bullets);
    }

    // A coroutine to change the velocity of the bullet to form a zigzag shape
    private IEnumerator ZigzagPattern(Bullet bullet, float initialAngle, float interval, float angleIncrement)
    {
        // The current angle of the bullet
        float angle = initialAngle + angleIncrement / 2;

        // The speed of the bullet
        float speed = bullet.velocity.magnitude;

        // Repeat until the bullet is destroyed
        while (bullet != null)
        {
            // Wait for the interval
            yield return new WaitForSeconds(interval);

            // Alternate the angle increment between positive and negative
            angleIncrement = -angleIncrement;

            // Increment the angle
            angle += angleIncrement;

            // Calculate the new velocity based on the angle and speed
            Vector2 velocity = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * speed;

            // Set the new velocity to the bullet
            bullet.velocity = velocity;
        }
    }
}
