//https://qiita.com/okuhiiro/items/3d69c602b8538c04a479

using System;
using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Type t = typeof(T);

                instance = (T)FindObjectOfType(t);
                if (instance == null)
                {
                    Debug.LogError(t + " をアタッチしているGameObjectはありません");
                }
            }

            return instance;
        }
    }

    virtual protected void Awake()
    {
        // 麿のGameObjectにアタッチされているか?{べる.
        // アタッチされている??栽は篤??する.
        if (this != Instance)
        {
            //Destroy(this);
            this.gameObject.SetActive(false);
            Destroy(this.gameObject);
            Debug.LogError(
                typeof(T) +
                " は屡に麿のGameObjectにアタッチされているため、コンポ?`ネントを篤??しました." +
                " アタッチされているGameObjectは " + Instance.gameObject.name + " です.");
            return;
        }

        // なんとかManager議なSceneを睡いでこのGameObjectを嗤?燭砲靴燭???栽は
        // ◎コメントアウト翌してください.
        //DontDestroyOnLoad(this.gameObject);
    }

}