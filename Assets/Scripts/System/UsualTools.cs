using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsualTools : MonoBehaviour
{
    public static void DeleteAllChildren(Transform go)
    {
        for (int i = 0; i < go.childCount; i++)
            Destroy(go.GetChild(i).gameObject);
    }

    public static Transform FindNearestEnemy(Transform checkObject, float trackRange)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(checkObject.position, trackRange, LayerMask.GetMask("Enemy"));
        Transform tmp = null;
        float minDistance = trackRange;
        foreach (Collider2D tmpCollider in colliders)
        {
            float distance = Vector2.Distance(tmpCollider.transform.position, checkObject.position);
            if (distance < minDistance)
            {
                tmp = tmpCollider.transform;
                minDistance = distance;
            }
        }
        return tmp;
    }

    public static Transform GetRandomEnemy()
    {
        int enemyNum = LevelManager.enemyContainer.childCount;
        return LevelManager.enemyContainer.GetChild(Random.Range(0, enemyNum));
    }

    public static bool RandomEvent(int triggerLimit, int randomRange)
    {
        return Random.Range(0, randomRange) <= triggerLimit;
    }

    public static bool RandomEvent(float triggerLimit, float randomRange)
    {
        return Random.Range(0, randomRange) <= triggerLimit;
    }
}
