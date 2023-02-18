using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    public string targetScene;
    public string targetDoor;

    public Transform spawnPivot;

    public bool isDefaultDoor;

    GameObject doorVisuals;
    bool isOpen = false;

    int doorLayer, obstacleLayer;

    // Start is called before the first frame update
    void Start()
    {
        if (!isOpen)
        {
            doorVisuals = new GameObject();
            doorVisuals.transform.parent = transform;
            doorVisuals.transform.Translate(Vector3.forward * 4);

            var sr = doorVisuals.AddComponent<SpriteRenderer>();
            sr.sprite = GameController.Instance.doorSprite;

            var coll = GetComponent<BoxCollider2D>();
            doorVisuals.transform.localPosition = coll.offset;
            doorVisuals.transform.localScale = new Vector3(coll.size.x, coll.size.y, 1.0f);
        }

        doorLayer = LayerMask.NameToLayer("Door");
        obstacleLayer = LayerMask.NameToLayer("Obstacles");
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen) { gameObject.layer = doorLayer; }
        else { gameObject.layer = obstacleLayer; }

        if(isOpen && doorVisuals != null)
        {
            doorVisuals.GetComponent<SpriteRenderer>().DOColor(Color.clear, 0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOpen) return;
        if(collision.gameObject == GameController.Instance.player)
        {
            GameController.Instance.EnterLevelAsync(targetScene, targetDoor);
        }
    }

    public void Open()
    {
        isOpen = true;
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
