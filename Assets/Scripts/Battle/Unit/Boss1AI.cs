using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Boss1State
{
    Break,
    Attack,
    Chase,
    Shrink,

}
public class Boss1AI : BaseAI
{
    public TarodevController.PlayerController body;
    public float attack1Interval = 1.0f;
    public float attack2Interval = 1.0f;
    public Vector2 direction;

    private Transform breakPos;
    private Boss1State state; //enemy state
    private float stateTime; 
    private float attack1Timer;
    private float attack2Timer;

    private float size_1 = 0.4f;
    private float size_2 = 0.7f;
    private float currScale = 1.0f;
    private float shrinkRate = 0.0008f;
    private int actionCounter = 0;

    public float floatSpeed = 4f; // float speed on axis y
    public float flySpeed = 2f; // fly speed on axis x
    private float centerY;
    private float breakTimer = 0.2f;
    

    protected void Awake()
    {
        if(target == null)
        {
            target = GameObject.FindWithTag(targetTag);
            if(target == null)
            {
                Debug.LogError($"Cannot find gameObject with tag {targetTag}!");
            }
        }

        stateTime = 2.0f;
        state = Boss1State.Attack;
        breakPos = transform;

        centerY = 0f;

        direction = direction.normalized;
    }

    protected override void Update()
    {

        stateTime -= Time.deltaTime;

        if (state == Boss1State.Break)
        {
            breakTimer -= Time.deltaTime;

            if(breakTimer < -0.2f){
                breakTimer = 0.2f;
            }
            Vector3 scaleChange = new Vector3(shrinkRate, shrinkRate, shrinkRate);
            if (breakTimer < 0) {
                transform.localScale += scaleChange;
            }
            else {
                transform.localScale -= scaleChange;
            }


            var currInput = Input;
            currInput.X = 0.0f;
            currInput.PrimaryFire = false;
            currInput.SecondaryFire = false;
            Input = currInput;
        }
        else if (state == Boss1State.Shrink)
        {
            Vector3 scaleChange = new Vector3(shrinkRate, shrinkRate, shrinkRate);
            transform.localScale += scaleChange;
            currScale += shrinkRate;

            if(currScale < 0.4)
            {
                stateTime = 0;
            }

            body.ApplyExternalMovement(new Vector2(0.00f, 0.008f));

            var currInput = Input;

            currInput.X = 0.0f;
            currInput.PrimaryFire = false;
            currInput.SecondaryFire = false;
            Input = currInput;
        }
        else if (state == Boss1State.Attack)
        {

            attack1Timer -= Time.deltaTime;

            var currInput = Input;
            currInput.X = direction.x;
            currInput.Y = direction.y;
            currInput.PrimaryFire = attack1Timer <= 0;
            currInput.SecondaryFire = false;
            Input = currInput;

            if(attack1Timer <= 0) { attack1Timer += attack1Interval; }
        }
        else
        {
            var currInput = Input;
            
            if(target == null)
            {
                target = GameObject.FindWithTag(targetTag);
                if(target == null)
                {
                    Debug.LogError($"Cannot find gameObject with tag {targetTag}!");
                }
            }

            float distance = Vector3.Distance(target.transform.position, transform.position);
            if(state == Boss1State.Chase && Vector3.Distance(target.transform.position, transform.position) < 2.0f)
            {
                state = Boss1State.Attack;
            }

            if(transform.position.x > target.transform.position.x && transform.position.x > -18.0f)
            {
                body.ApplyExternalMovement(new Vector2(-0.01f, (Random.value -0.55f) * 0.01f));
            }
            else
            {
                body.ApplyExternalMovement(new Vector2(0.01f, (Random.value -0.55f) * 0.01f));
            }


            currInput.X = (Random.value - 0.5f);
            currInput.Y = 1;
            currInput.PrimaryFire = false;
            currInput.SecondaryFire = true;
            Input = currInput;

            if(attack2Timer <= 0) { attack2Timer += attack2Interval; }
        }

        if (stateTime < 0)
        {
            state = NextState(state);
            Debug.Log(state);
        }
       
    }
    private Boss1State NextState(Boss1State laststate)
    {
        if(laststate == Boss1State.Break)
        {   
            stateTime = 1.5f;
            return Boss1State.Shrink;
        }
        if(actionCounter-- < 0)
        {
            actionCounter = 3;
            stateTime = 1.5f;
            return Boss1State.Break;
        }
        else
        {
            float p = Random.value;
            if (p < 0.3)
            {
                stateTime = 3.0f;
                return Boss1State.Attack;
            }
            else
            {
                stateTime = 3.0f;
                return Boss1State.Chase;
            }
        }
    }
}
