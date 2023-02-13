using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackPatternBase : MonoBehaviour
{
    public abstract void Fire(Vector2 origin, Vector2 direction);
}
