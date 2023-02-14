using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab;

    public int spawnCount = 1;
    public float spawnInterval = 0;
    public float initialWaitingTime = 0;

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
            Instantiate(enemyPrefab, transform);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "EnemySpawn.png", true);
    }
}
