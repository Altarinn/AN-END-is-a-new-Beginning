using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAI : InputModule
{
    public GameObject target;
    public string targetTag = "TruePlayer";

    // Start is called before the first frame update
    protected abstract void Start();

    // Update is called once per frame
    protected abstract void Update();
}
