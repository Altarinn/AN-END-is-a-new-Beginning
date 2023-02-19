using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcanoFire : PlayerInputHandler
{
    public AttackPatternBase Fire1;

    public float originHeight = 1.0f;

    private Vector2 direction;

    protected override void HandleInput(FrameInput input)
    {
        float angle = Random.Range(15f, 165f); // generate a random angle
        float radians = angle * Mathf.Deg2Rad; // convert to radians
        direction = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians)); // calculate the direction of the velocity
        if (input.PrimaryFire)
        {
            Fire1?.Fire(transform.position + Vector3.up * originHeight, direction);
        }
    }
}

