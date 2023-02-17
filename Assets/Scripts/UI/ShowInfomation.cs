using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInfomation : MonoBehaviour
{
    public string tips;

    private GameObject textGo;

    private void Start()
    {
        textGo = UIManager.GetInstance().InstantiateTips(tips, transform.position + new Vector3(0, 1.4f, 0));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "TruePlayer")
        {
            textGo.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "TruePlayer")
        {
            textGo.SetActive(false);

        }
    }
}
