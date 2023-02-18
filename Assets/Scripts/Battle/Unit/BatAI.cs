using System;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Battle.Unit
{
    public enum BatState
    {
        Chase,
        Attack
    }
    public class BatAI : BaseAI
    {
        public TarodevController.PlayerController body;
        public float floatSpeed = 4f; // float speed on axis y
        public float flySpeed = 2f; // fly speed on axis x
        public float floatHeight = 0.5f; // float height on axis y
        public float attackDist = 3f; // while shorter than this distance, switch to attack mode 
        public float safeHeight = 2f; //a safe height that bot will try to keep while attack mode
        public float spamInterval = 2f;
                
        float spamTimer;
        private BatState state;
        private Vector2 move;
        private float centerY;


        protected override void Start()
        {
            base.Start();
            state = BatState.Chase;
            move = Vector2.zero;
            centerY = 0;
            spamTimer = 0;
        }

        //return the vector fly to a point above player
        private Vector2 straitChase()
        {
            Vector2 headPoint = new Vector2(target.transform.position.x,
                target.transform.position.y + safeHeight);
            Vector2 chaseVec = (headPoint - (Vector2)transform.position).normalized;
            return chaseVec;
        }

        protected override void Update()
        {
            //just use 
            var currentInput = Input;
            currentInput.Y = 0;

            Vector2 totalMove = Vector2.zero;
            float distance = Vector3.Distance(target.transform.position, transform.position);
            if(state == BatState.Chase && Vector3.Distance(target.transform.position, transform.position) < attackDist)
            {
                state = BatState.Attack;
            }

            // straight move to player
            Vector2 chaseVec = straitChase();
            totalMove += flySpeed * chaseVec * Vector2.up * Time.deltaTime;
            currentInput.X = chaseVec.x;

            if (state == BatState.Chase)
            {
                //calculate delta y in a sin func
                float sinY = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
                totalMove += new Vector2(0, sinY - centerY);
                centerY = sinY;
            }
            else
            {
                spamTimer -= Time.deltaTime;
                if(spamTimer < 0)
                {
                    currentInput.PrimaryFire = true;
                    spamTimer += spamInterval;
                }
                else
                {
                    currentInput.PrimaryFire = false;
                }
            }
            body.ApplyExternalMovement(totalMove);
            Input = currentInput;
        }


    }
}
