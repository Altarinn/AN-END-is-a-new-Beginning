using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Battle.Unit
{
    public enum VolcanoState
    {
        Dash,
        Rest,
        BigJump,
        Volcano
    }

    public class VolcanoAI : BaseAI
    {
        public TarodevController.PlayerController body;
        public float dashSpeed = 30f;
        public float restTime = 1f;
        public float volcanoTime = 5f;
        public float dashTime = 1f;
        public float spamInterval = 0.05f;

        float spamTimer;
        private bool volcanoDash;
        private float stateTime = 0;
        private VolcanoState state;
        private Vector2 move;
        
        //used to judge move to center
        private Vector3 screenCenter;
        public float centerOffset = 0.2f;
        
        private VolcanoState[] moveList = new[] { VolcanoState.Dash,VolcanoState.Dash, VolcanoState.Volcano };
        private int movePtr = 0;
        private Vector2 dashVec;

        protected override void Start()
        {
            base.Start();
            volcanoDash = false;
            state = VolcanoState.Rest;
            stateTime = restTime;
            screenCenter = new Vector3(Screen.width / 2, 0f, 0f);
            screenCenter = Camera.main.ScreenToWorldPoint(screenCenter);
            spamTimer = 0;
        }

        private bool judgeCenter()
        {
            return Mathf.Abs(transform.position.x - screenCenter.x) < centerOffset;
        }
        
        protected override void Update()
        {
            move = Vector2.zero;
            var currentInput = Input;
            currentInput.Y = 0; 
            currentInput.X = 0;
            currentInput.PrimaryFire = false;

            if (stateTime < 0)
            {
                if (state != VolcanoState.Rest)
                {
                    state = VolcanoState.Rest;
                    stateTime = restTime;
                }
                else
                {
                    state = moveList[movePtr];
                    movePtr = (movePtr + 1) % moveList.Length;
                    if (state == VolcanoState.Volcano)
                    {
                        stateTime = volcanoTime;
                        volcanoDash = true;    
                        dashVec = new Vector2(screenCenter.x - transform.position.x, 0).normalized;
                    }

                    if (state == VolcanoState.Dash)
                    {
                        stateTime = dashTime;
                        dashVec = new Vector2(target.transform.position.x - transform.position.x, 0).normalized;
                    }
                    
                }
            }

            switch (state)
            {
                case VolcanoState.Volcano:
                    if (volcanoDash)
                    {
                        if (judgeCenter())
                        {
                            volcanoDash = false;
                        }
                        else
                        {
                            move += dashVec * dashSpeed * Time.deltaTime;
                        }
                    }
                    else
                    {
                        stateTime -= Time.deltaTime;
                        spamTimer -= Time.deltaTime;
                        if (spamTimer < 0)
                        {
                            currentInput.PrimaryFire = true;
                            spamTimer += spamInterval;
                        }
                    }
                    break;
                case VolcanoState.Dash:
                    move += dashVec * dashSpeed * Time.deltaTime;
                    stateTime -= Time.deltaTime;
                    break;
                case VolcanoState.Rest:
                    stateTime -= Time.deltaTime;
                    break;
            }
            body.ApplyExternalMovement(move);
            Input = currentInput;
        }


    }
}
