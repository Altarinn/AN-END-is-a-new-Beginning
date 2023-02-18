using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    public GameObject HPBlockPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameController.Instance.player == null) { return; }

        var player = GameController.Instance.player;
        int currentHealth = Mathf.RoundToInt(player.GetComponent<DamageTaker>().health);

        if(currentHealth == transform.childCount) { return; }
        
        if(currentHealth > transform.childCount)
        {
            for(int i = transform.childCount; i < currentHealth; i++)
            {
                Instantiate(HPBlockPrefab, transform);
            }
        }
        else
        {
            for (int i = currentHealth; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
        }
    }
}
