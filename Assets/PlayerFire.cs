using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : PlayerInputHandler
{
    public AttackPatternBase Fire1, Fire2, Fire3;

    protected override void HandleInput(FrameInput input)
    {
        Vector2 direction = Vector2.right;

        if(input.PrimaryFire)
        {
            Fire1?.Fire(transform.position, direction);
        }
        if(input.SecondaryFire)
        {
            Fire2?.Fire(transform.position, direction);
        }
    }
}
