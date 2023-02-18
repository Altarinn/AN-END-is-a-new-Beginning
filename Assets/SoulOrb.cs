using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulOrb : MonoBehaviour
{
    public float flySpeed = 20.0f, turn = 360.0f;

    public void Go(Vector3 initVelo, Vector3 target)
    {
        StartCoroutine(Fly(initVelo, target));
    }

    IEnumerator Fly(Vector3 initVelocity, Vector3 target)
    {
        Vector3 velocity = initVelocity.normalized * flySpeed;

        while((transform.position - target).sqrMagnitude > 0.25)
        {
            yield return null;

            Vector3 optimalVelo = (target - transform.position).normalized * flySpeed;
            velocity = Vector3.RotateTowards(velocity, optimalVelo, turn * Mathf.Deg2Rad * Time.deltaTime, flySpeed * Time.deltaTime);
            transform.position += velocity * Time.deltaTime;
        }

        Destroy(gameObject);
    }
}
