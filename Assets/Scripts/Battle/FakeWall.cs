using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FakeWall : MonoBehaviour
{
    private Tilemap tilemap;
    private float difuseSpeed = 1;
    private bool showing = true;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void Update()
    {
        if (showing)
        {
            if (tilemap.color.a < 1)
                tilemap.color += new Color(0, 0, 0, tilemap.color.a + Time.deltaTime * difuseSpeed);
        }
        else
        {
            if (tilemap.color.a > 0)
                tilemap.color -= new Color(0, 0, 0, tilemap.color.a + Time.deltaTime * difuseSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GameController.Instance.player)
        {
            showing = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == GameController.Instance.player)
        {
            showing = true;
        }
    }
}
