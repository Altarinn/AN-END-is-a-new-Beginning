using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoManager : MonoBehaviour
{
    private static EchoManager _instance = null;

    private EchoManager()
    {
        _instance = this;
    }

    public static EchoManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new EchoManager();
        }
        return _instance;
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
