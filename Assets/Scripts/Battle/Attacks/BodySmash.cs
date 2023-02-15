using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TarodevController.PlayerController))]
public class BodySmash : MonoBehaviour
{
    public float damage;

    TarodevController.PlayerController playerController;

    [Header("Knockback (attacker)")]
    public float knockbackSpeed = 15.0f;
    public float knockbackTime = 0.2f;
    Vector2 knockbackVelocity = Vector2.zero;

    private void Awake()
    {
        playerController = GetComponent<TarodevController.PlayerController>();
    }

    private void Update()
    {
        if (knockbackVelocity.sqrMagnitude > 1e-4)
        {
            playerController.ApplyExternalMovement(knockbackVelocity * Time.deltaTime);
            knockbackVelocity -= knockbackVelocity.normalized * knockbackSpeed / knockbackTime * Time.deltaTime;
        }
        else
        {
            knockbackVelocity = Vector2.zero;
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        var dt = collider.GetComponent<DamageTaker>();
        if (dt != null)
        {
            dt.Damage(damage, playerController.Velocity.normalized);
            knockbackVelocity = playerController.Velocity.normalized * -1 * knockbackSpeed;
        }
    }
}
