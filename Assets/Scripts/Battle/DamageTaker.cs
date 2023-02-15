using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DamageTaker : MonoBehaviour
{
    [Header("Essentials"), Tooltip("This should be a child of the gameObject this component is attached with localScale 1,1,1.")]
    public GameObject visuals;
    public TarodevController.PlayerController body;
    public SpriteRenderer spriteRenderer;

    Transform vT;

    Vector3 vTorigin;
    Color vRcolor;

    private MaterialPropertyBlock propertyBlock;

    [Header("Health")]
    public float maxHealth;
    public float health;
    public bool destroyOnDeath = true, dead = false;

    [Header("Damage react")]
    public float damageTime = 0.12f;
    public float recoverTime = 0.3f, invincibleTime = 0.25f;
    
    float invincibleTimer = 0.0f;
    public bool Invincible { get { return invincibleTimer > 0.0f; } }

    [Header("Knockback")]
    public float knockbackSpeed = 5.0f;
    public float knockbackTime = 0.5f;
    Vector2 knockbackVelocity = Vector2.zero;

    Sequence seq;

    private void Awake()
    {
        vT = visuals.transform;
        vTorigin = vT.localPosition;

        vRcolor = spriteRenderer.color;

        propertyBlock = new MaterialPropertyBlock();
        //propertyBlock.SetFloat("Damaged", 0);
        //propertyBlock.SetFloat("Healed", 0);

        //spriteRenderer.SetPropertyBlock(propertyBlock);

        health = maxHealth;
        dead = false;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Invincible)
        {
            invincibleTimer -= Time.deltaTime;
        }

        if(knockbackVelocity.sqrMagnitude > 1e-4)
        {
            body.ApplyExternalMovement(knockbackVelocity * Time.deltaTime);
            knockbackVelocity -= knockbackVelocity.normalized * knockbackSpeed / knockbackTime * Time.deltaTime;
        }
        else
        {
            knockbackVelocity = Vector2.zero;
        }
    }

    [ContextMenu("Damage test")]
    void DamageTest() => Damage(10, Vector2.left);

    /// <summary>
    /// DamageTaker will take damage and react to the damage.
    /// </summary>
    /// <param name="damage">damage value</param>
    /// <param name="direction">where the damage comes from</param>
    public void Damage(float damage, Vector2 direction)
    {
        if (Invincible) return;
        if (damage <= 0) return;

        health -= damage;
        CheckDeath();

        direction = new Vector2(Mathf.Sign(direction.x), 0);
        //direction = new Vector2(0, 0);

        // Disable player input
        body.UseInput = false;
        body.ZeroVelocity();

        seq?.Kill();
        seq = DOTween.Sequence()
            //.Append(vT.DOLocalMove(vTorigin + (Vector3)direction, damageTime))
            .Append(vT.DOScale(0.8f, damageTime))
            .Join(DOVirtual.Float(0, 1, damageTime, (val) =>
            {
                spriteRenderer?.GetPropertyBlock(propertyBlock, 0);
                propertyBlock.SetTexture("_MainTex", spriteRenderer.sprite.texture);
                propertyBlock.SetFloat("_Damaged", val);
                spriteRenderer?.SetPropertyBlock(propertyBlock, 0);
            }).SetEase(Ease.OutQuart))
            .Join(DOVirtual.DelayedCall(damageTime, () => body.UseInput = true))

            //.Append(vT.DOLocalMove(vTorigin, recoverTime))
            .Append(vT.DOScale(1.0f, recoverTime))
            .Join(DOVirtual.Float(1, 0, recoverTime, (val) =>
            {
                spriteRenderer?.GetPropertyBlock(propertyBlock, 0);
                propertyBlock.SetTexture("_MainTex", spriteRenderer.sprite.texture);
                propertyBlock.SetFloat("_Damaged", val);
                spriteRenderer?.SetPropertyBlock(propertyBlock, 0);
            }));

        invincibleTimer = invincibleTime;
        knockbackVelocity = direction * knockbackSpeed;
    }

    public void Heal(float healing)
    {

    }

    void CheckDeath()
    {
        if(health < 0)
        {
            dead = true;
            if (destroyOnDeath) { seq?.Kill(); Destroy(this.gameObject); }
        }
    }
}
