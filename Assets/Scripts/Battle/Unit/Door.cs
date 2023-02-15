using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public string targetScene;
    public string targetDoor;

    public Transform spawnPivot;

    public bool isDefaultDoor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == GameController.Instance.player)
        {
            GameController.Instance.EnterLevelAsync(targetScene, targetDoor);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(spawnPivot.position, "PlayerSpawn.png", true);

        var boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            var color = Gizmos.color;
            Gizmos.color = new Color(1.0f, 0.8f, 0.4f);

            Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.size);

            Gizmos.color = color;
        }
    }
}
