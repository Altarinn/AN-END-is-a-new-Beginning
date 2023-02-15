using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyState
{
    Rest,
    Wander,
    Chase
}
public class CreepAI : BaseAI
{
    public Transform frontRect;
    public float freeSpeed = 1.0f; 
    public float idleTime = 3.0f; 
    public float chaseTime = 3.0f; // time keep chasing while not in sight
    public float restRate = 0.35f;
    public float WanderRate = 0.65f;


    private EnemyState state; //enemy state
    private float restTime;
    private float stateTime; 

    protected override void Start()
    {
        restTime = 0.0f;
        stateTime = 0.0f;
        state = EnemyState.Rest;
    }

    protected override void Update()
    {
        // detect whether player in sight
        bool isTargetInSight = frontRect != null && Physics2D.OverlapBox(frontRect.position, frontRect.lossyScale, 0.0f, LayerMask.GetMask("Player")) != null;

        stateTime -= Time.deltaTime;
        //judge state
        if (isTargetInSight)
        {
            if (state!=EnemyState.Chase)
            {
                state = EnemyState.Chase;
            }
            // reset chase time
            stateTime = chaseTime;
        }

        // random state while not chasing
        if (stateTime<0)
        {
            state = RandomState();
            if (state == EnemyState.Wander)
            {
                if (Random.value > 0.5)
                {
                    Vector3 scale = transform.localScale;
                    scale.x *= -1;
                    transform.localScale = scale;
                }
            }
            stateTime = Random.value * idleTime;
        }

        if (state == EnemyState.Rest)
        {
                var currInput = Input;
                currInput.X = 0.0f;
                Input = currInput;
        }
        else
        {
            var currInput = Input;
            currInput.X = -1.0f * transform.localScale.x;
            Input = currInput;
        }
       
    }

    private EnemyState RandomState()
    {
        float p = Random.value;

        if (p < restRate)
        {
            return EnemyState.Rest;  
        }
        else
        {
            return EnemyState.Wander; 
        }
    }
}
