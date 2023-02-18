using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatFire : PlayerInputHandler
{
    public AttackPatternBase Fire1;

    private Vector2 direction = Vector2.down;

    protected override void HandleInput(FrameInput input)
    {
        //if (input.X != 0.0f)
        //{ 
        //    direction = Vector2.right * input.X;
        //}

        if (input.PrimaryFire)
        {
            Fire1?.Fire(transform.position, direction);
        }
    }
}
