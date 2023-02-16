using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepAI : BaseAI
{
    protected override void Update()
    {
        var currInput = Input;

        if(transform.position.x > target.transform.position.x)
        {
            currInput.X = -1.0f;
        }
        else
        {
            currInput.X = 1.0f;
        }

        Input = currInput;
    }
}
