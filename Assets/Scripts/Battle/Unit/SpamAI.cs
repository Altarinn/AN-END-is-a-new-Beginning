using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Keeps firing and do nothing else.
public class SpamAI : BaseAI
{
    public Vector2 direction;

    public float spamInterval = 1.0f;
    float spamTimer = 0;

    private void Awake()
    {
        direction = direction.normalized;
    }

    protected override void Update()
    {
        spamTimer -= Time.deltaTime;

        var currInput = Input;
        currInput.X = direction.x;
        currInput.Y = direction.y;
        currInput.PrimaryFire = spamTimer <= 0;
        Input = currInput;

        if(spamTimer <= 0) { spamTimer += spamInterval; }
    }
}
