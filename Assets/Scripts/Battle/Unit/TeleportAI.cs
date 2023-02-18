using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportAI : BaseAI
{
    public float VerticalHomingSpeed = 1.0f;
    TarodevController.PlayerController controller;
    public float TeleTime = 5;
    private float TeleTimer;
    public Transform[] TelePos;
    private Animator anim;
    private int PosNum = -1;

    private void Awake()
    {
        controller = GetComponent<TarodevController.PlayerController>();
        anim = GetComponent<Animator>();
        //TeleTimer = TeleTime;
    }

    protected override void Update()
    {
        var currInput = Input;

        //if(transform.position.x > target.transform.position.x)
        //{
        //    currInput.X = -1.0f;
        //}
        //else
        //{
        //    currInput.X = 1.0f;
        //}

        DoTeleport();
        //if (anim.GetCurrentAnimatorStateInfo(0).IsName("BeforeAttack"))
        //{
        //    TeleFlag = true;

        //}
        //if (TeleFlag)
        //{

        //    TeleFlag = false;
        //}

        currInput.PrimaryFire = false;


        //if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        //{
        //    //if (transform.position.x > target.transform.position.x)
        //    //{
        //    //    currInput.X = -1.0f;
        //    //}
        //    //else
        //    //{
        //    //    currInput.X = 1.0f;
        //    //}

        //}
        //if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        //{
        //    currInput.PrimaryFire = false;
        //}


        //controller.ApplyExternalMovement(Vector2.up * Mathf.Sign(target.transform.position.y - transform.position.y) * Time.deltaTime);

        Input = currInput;
    }

    private void DoTeleport()
    {
        TeleTimer -= Time.deltaTime;
        if (TeleTimer > 0)
            return;
        TeleTimer = TeleTime;
        anim.SetTrigger("Attack");
    }

    public void changePos()
    {
        int p = Random.Range(0, TelePos.Length);
        while (p==PosNum)
            p = Random.Range(0, TelePos.Length);
        this.transform.position = TelePos[p].position;
        PosNum = p;
    }

    public void doFire()
    {
        var currInput = Input;
        currInput.Y = 0;
        currInput.Y = -1;
        currInput.PrimaryFire = true;
        Input = currInput;
    }

}
