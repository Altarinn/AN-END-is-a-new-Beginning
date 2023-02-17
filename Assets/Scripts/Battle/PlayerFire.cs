using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : PlayerInputHandler
{
    public AttackPatternBase Fire1, Fire2, Fire3;

    private Vector2 direction = Vector2.right;

    protected override void HandleInput(FrameInput input)
    {
        //if (input.X != 0.0f)
        //{ 
        //    direction = Vector2.right * input.X;
        //}
        direction = new Vector2(input.X, input.Y);
        
        if(input.PrimaryFire)
        {
            Fire1?.Fire(transform.position, direction);
        }
        if(input.SecondaryFire)
        {
            Fire2?.Fire(transform.position, direction);
        }
        if(input.ThirdFire)
        {
            Fire3?.Fire(transform.position, direction);
        }
    }
}
