using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Sprite))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider2D))]

public abstract class BulletBase : MonoBehaviour
{
    ///<summary>
    ///�ӵ�ID
    ///</summary>
    [Tooltip("�ӵ�id")]
    public string BulletId;

    ///<summary>
    ///�ӵ��ٶ�
    ///</summary>
    [Tooltip("�ӵ��ٶ�")]
    //[Tooltip("������Բ���")]
    public float Speed = 2;

    ///<summary>
    ///�ӵ����
    ///</summary>
    [Tooltip("�ӵ����")]
    //[Tooltip("������Բ���")]
    public float Distance = 20;

    ///<summary>
    ///�ӵ�����
    ///</summary>
    [Tooltip("�ӵ�����")]
    //[Tooltip("������Բ���")]
    public BulletType Type;

    ///<summary>
    ///����buff
    ///</summary>
    [Tooltip("����buff")]
    //[Tooltip("���к���ӵ�buff")]
    public int[] Buffs;

    ///<summary>
    ///ʣ����д���
    ///</summary>
    [Tooltip("���д���")]
    //[Tooltip("�ӵ��ڻ��ж�Ӧ�Ĵ�������ʧ��������Բ���")]
    public int HitNumber = 1;

    ///<summary>
    ///��ЧƵ��
    ///</summary>
    [Tooltip("��ЧƵ��")]
    //[Tooltip("������ܶ�ͬһ����λ�ٴ���Ч���Լ���\n���Ծ��Ǵ���Ƶ�ʣ���д��λΪ����")]
    public float HitFrequency = 1000;

    /// <summary>
    /// �ӵ��Ƕ�
    /// </summary>
    [HideInInspector]
    public float Degree;

    ///<summary>
    ///������
    ///</summary>
    protected Animator animator;

    /// <summary>
    /// ��ʼλ��
    /// </summary>
    protected Vector2 startPos;

    /// <summary>
    /// ���䵥λ
    /// </summary>
    //[HideInInspector] public Unit unit;

    private Queue<GameObject> protectedEnemy = new Queue<GameObject>();
    private Queue<float> triggerTime = new Queue<float>();

    virtual protected void Start()
    {
        ParamInit();
    }

    virtual protected void Update()
    {
        MoveMethod();
        CheckPos();
    }

    ///<summary>
    ///��ʼ��
    ///</summary>
    virtual protected void ParamInit()
    {
        animator = GetComponent<Animator>();
        startPos = transform.position;
    }

    ///<summary>
    ///�ƶ���ʽ
    ///</summary>
    abstract protected void MoveMethod();

    ///<summary>
    ///���е���
    ///</summary>
    virtual protected void OnSmash()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Animator>().SetTrigger("Hit");
        this.enabled = false;
    }

    virtual protected void OnHit(Collider2D collision)
    {
        // TODO
        //BuffManager tmpBuffManager = collision.gameObject.GetComponent<BuffManager>();
        //if (tmpBuffManager == null)
        //    return;
        //tmpBuffManager.AddBuffToQueue(Buffs, unit);

        HitNumber--;
        if (HitNumber <= 0)
            OnSmash();
    }

    ///<summary>
    ///���λ��
    ///</summary>
    public virtual void CheckPos()
    {
        if (Vector3.Distance(transform.position, startPos) >= Distance)
        {
            OnSmash();
        }

        // TODO
        //if (transform.position.x <= -LevelManager.bulletWidthLimit || transform.position.x >= LevelManager.bulletWidthLimit)
        //    OnSmash();
        //if (transform.position.y <= LevelManager.bulletBottomLimit || transform.position.y >= LevelManager.bulletHeightLimit)
        //    OnSmash();
    }

    public virtual void OnAnimationOver()
    {
        Destroy(gameObject);
    }

    public BulletBase() { }

    virtual protected void OnTriggerStay2D(Collider2D collision)
    {
        while (triggerTime.Count > 0)
        {
            if (Time.fixedTime - triggerTime.Peek() >= HitFrequency)
            {
                triggerTime.Dequeue();
                protectedEnemy.Dequeue();
            }
            else
            {
                break;
            }
        }
        if (protectedEnemy.Contains(collision.gameObject))
            return;
        OnHit(collision);
        protectedEnemy.Enqueue(collision.gameObject);
        triggerTime.Enqueue(Time.fixedTime);
    }
}
public enum BulletType
{
    ///<summary>
    ///ֱ���ӵ�
    ///</summary>
    [InspectorName("ֱ���ӵ�")]
    Linear,
    
    ///<summary>
    ///׷���ӵ�
    ///</summary>
    [InspectorName("׷���ӵ�")]
    Track,
    
    ///<summary>
    ///���ٱ���ӵ�
    ///</summary>
    [InspectorName("���ٱ���ӵ�")]
    ChangableBullet,
    
    ///<summary>
    ///����
    ///</summary>
    [InspectorName("����")]
    Laser,
}