using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingAI : BaseAI
{
    public float VerticalHomingSpeed = 1.0f;
    TarodevController.PlayerController controller;

    private void Awake()
    {
        controller = GetComponent<TarodevController.PlayerController>();
    }

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

        controller.ApplyExternalMovement(Vector2.up * Mathf.Sign(target.transform.position.y - transform.position.y) * Time.deltaTime);

        Input = currInput;
    }
}
