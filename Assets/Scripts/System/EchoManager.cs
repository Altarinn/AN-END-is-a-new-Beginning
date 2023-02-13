using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoManager : SingletonMonoBehaviour<EchoManager>
{
    public static EchoManager GetInstance() => Instance;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
}
