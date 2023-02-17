using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BarrierDamagerTaker : DamageTaker
{
    [Header("Respawn Paramater")]
    public float respawnTime = 3f;

    private Collider2D col2D;
    private float deadTimer;

    protected override void Awake()
    {
        vT = visuals.transform;

        col2D = GetComponent<Collider2D>();

        //propertyBlock.SetFloat("Damaged", 0);
        //propertyBlock.SetFloat("Healed", 0);

        //spriteRenderer.SetPropertyBlock(propertyBlock);

        health = maxHealth;
        dead = false;
    }

    protected override void Update()
    {
        if (dead)
        {
            deadTimer -= Time.deltaTime;
            if(deadTimer < 0)
            {
                col2D.enabled = true;
                health = maxHealth;
                spriteRenderer.color = new Color(1, 1, 1, 1);
                dead = false;
            }
        }
    }

    public override void Damage(float damage, Vector2 direction)
    {
        if (damage <= 0) return;

        health -= damage;
        spriteRenderer.color = new Color(1, 1, 1, health / maxHealth);
        CheckDeath();
    }

    protected override void CheckDeath()
    {
        if (health < 0)
        {
            dead = true;
            if (destroyOnDeath) { seq?.Kill(); Destroy(this.gameObject); }
            else
            {
                col2D.enabled = false;
                deadTimer = respawnTime;
            }
        }
    }
}
