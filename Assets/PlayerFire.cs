using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : PlayerInputHandler
{
    public GameObject bulletPrefab;

    protected override void HandleInput(FrameInput input)
    {
        if(input.PrimaryFire)
        {
            Fire(transform.position);
        }
    }

    void Fire(Vector2 pos)
    {
        SpriteRenderer test = Instantiate(bulletPrefab, new Vector3(pos.x, pos.y, -2.0f), Quaternion.identity).GetComponent<SpriteRenderer>();
        test.color = Random.ColorHSV();
    }
}
