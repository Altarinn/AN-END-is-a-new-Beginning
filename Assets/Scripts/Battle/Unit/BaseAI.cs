using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAI : InputModule
{
    public GameObject target;
    public string targetTag = "TruePlayer";

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if(target == null)
        {
            target = GameObject.FindWithTag(targetTag);
            if(target == null)
            {
                Debug.LogError($"Cannot find gameObject with tag {targetTag}!");
            }
        }
    }

    // Update is called once per frame
    protected abstract void Update();

    protected virtual void LateUpdate()
    {
        if (target == null)
        {
            target = GameObject.FindWithTag(targetTag);
        }
    }
}
