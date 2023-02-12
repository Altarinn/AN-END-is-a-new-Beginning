using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Sprite))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider2D))]

public abstract class BulletBase : MonoBehaviour
{
    ///<summary>
    ///子弹ID
    ///</summary>
    [Tooltip("子弹id")]
    public string BulletId;

    ///<summary>
    ///子弹速度
    ///</summary>
    [Tooltip("子弹速度")]
    //[Tooltip("激光可以不管")]
    public float Speed = 2;

    ///<summary>
    ///子弹射程
    ///</summary>
    [Tooltip("子弹射程")]
    //[Tooltip("激光可以不管")]
    public float Distance = 20;

    ///<summary>
    ///子弹类型
    ///</summary>
    [Tooltip("子弹类型")]
    //[Tooltip("激光可以不管")]
    public BulletType Type;

    ///<summary>
    ///击中buff
    ///</summary>
    [Tooltip("击中buff")]
    //[Tooltip("击中后添加的buff")]
    public int[] Buffs;

    ///<summary>
    ///剩余击中次数
    ///</summary>
    [Tooltip("击中次数")]
    //[Tooltip("子弹在击中对应的次数后消失，激光可以不管")]
    public int HitNumber = 1;

    ///<summary>
    ///生效频率
    ///</summary>
    [Tooltip("生效频率")]
    //[Tooltip("隔多久能对同一个单位再次生效，对激光\n而言就是触发频率，填写单位为毫秒")]
    public float HitFrequency = 1000;

    /// <summary>
    /// 子弹角度
    /// </summary>
    [HideInInspector]
    public float Degree;

    ///<summary>
    ///动画机
    ///</summary>
    protected Animator animator;

    /// <summary>
    /// 起始位置
    /// </summary>
    protected Vector2 startPos;

    /// <summary>
    /// 发射单位
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
    ///初始化
    ///</summary>
    virtual protected void ParamInit()
    {
        animator = GetComponent<Animator>();
        startPos = transform.position;
    }

    ///<summary>
    ///移动方式
    ///</summary>
    abstract protected void MoveMethod();

    ///<summary>
    ///击中调用
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
    ///检查位置
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
    ///直线子弹
    ///</summary>
    [InspectorName("直线子弹")]
    Linear,
    
    ///<summary>
    ///追踪子弹
    ///</summary>
    [InspectorName("追踪子弹")]
    Track,
    
    ///<summary>
    ///变速变角子弹
    ///</summary>
    [InspectorName("变速变角子弹")]
    ChangableBullet,
    
    ///<summary>
    ///激光
    ///</summary>
    [InspectorName("激光")]
    Laser,
}