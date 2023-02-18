using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class DeathExplosion : MonoBehaviour
{
    public SpriteRenderer circle;

    // Start is called before the first frame update
    void Start()
    {
        circle.DOColor(new Color(1f, 0.0f, 0.2f, 0.0f), 0.3f).SetDelay(0.15f);
        Destroy(gameObject, 1.0f);
    }
}
