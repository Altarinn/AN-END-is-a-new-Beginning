using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab;

    public int spawnCount = 1;
    public float spawnInterval = 0;
    public float initialWaitingTime = 0;

    public bool spawnFinished { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(initialWaitingTime);

        for(int i = 0; i < spawnCount; i++)
        {
            SpriteRenderer renderer = Instantiate(enemyPrefab, transform).GetComponent<DamageTaker>().spriteRenderer;
            renderer.color = Color.clear;
            renderer.DOColor(Color.white, 0.3f);
            yield return new WaitForSeconds(spawnInterval);
        }

        spawnFinished = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "EnemySpawn.png", true);
    }
}
