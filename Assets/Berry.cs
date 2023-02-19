using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berry : MonoBehaviour
{
    public int id;

    public bool IsBerry;
    public bool IsBomb;

    private void Awake()
    {
        if(GameController.Instance.ItemObtained(id))
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == GameController.Instance.player)
        {
            GameController.Instance.GetItem(id);
            if (IsBerry) { GameController.Instance.GetBerry(); }
            if (IsBomb) { GameController.Instance.GetBomb(); }

            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
