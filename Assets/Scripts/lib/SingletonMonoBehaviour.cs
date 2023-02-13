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
                    Debug.LogError(t + " �򥢥��å����Ƥ���GameObject�Ϥ���ޤ���");
                }
            }

            return instance;
        }
    }

    virtual protected void Awake()
    {
        // ����GameObject�˥����å�����Ƥ��뤫�{�٤�.
        // �����å�����Ƥ�����Ϥ��Ɨ�����.
        if (this != Instance)
        {
            //Destroy(this);
            Destroy(this.gameObject);
            Debug.LogError(
                typeof(T) +
                " �ϼȤ�����GameObject�˥����å�����Ƥ��뤿�ᡢ����ݩ`�ͥ�Ȥ��Ɨ����ޤ���." +
                " �����å�����Ƥ���GameObject�� " + Instance.gameObject.name + " �Ǥ�.");
            return;
        }

        // �ʤ�Ȥ�Manager�Ĥ�Scene��礤�Ǥ���GameObject���Є��ˤ��������Ϥ�
        // �������ȥ������⤷�Ƥ�������.
        //DontDestroyOnLoad(this.gameObject);
    }

}