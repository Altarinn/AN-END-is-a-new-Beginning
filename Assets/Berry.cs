using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berry : MonoBehaviour
{
    public int id;

    public bool IsBerry;
    public bool IsBomb;

    Vector3 origin;

    private void Awake()
    {
        if(GameController.Instance.ItemObtained(id))
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        origin = transform.position;
    }

    private void Update()
    {
        transform.position = origin + 0.2f * Vector3.up * Mathf.Sin(Time.time);
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
