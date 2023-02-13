using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FrameInput
{
    public float X, Y;
    public bool JumpDown;
    public bool JumpUp;

    public bool PrimaryFire;

    public bool beingHit; // Animation trigger (placeholder)
}

public class ReplayableInput : MonoBehaviour
{
    public FrameInput Input { get; private set; }

    float fireCounter;

    [SerializeField]
    float fireIntervalMS = 100.0f;

    private void Awake()
    {
        fireCounter = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        GatherInput();
    }

    private void GatherInput()
    {
        // Handle sustained input as multiple repeated inputs
        bool isFire1 = false;

        // TODO: Change to fixed 60FPS and int fireCounter?
        if(fireCounter >= 0) { fireCounter -= Time.deltaTime; }

        if(fireCounter < 0 && UnityEngine.Input.GetButton("Fire1"))
        {
            isFire1 = true;
            fireCounter += fireIntervalMS / 1000.0f;
        }
        //isFire1 = UnityEngine.Input.GetButtonDown("Fire1");

        Input = new FrameInput
        {
            JumpDown = UnityEngine.Input.GetButtonDown("Jump"),
            JumpUp = UnityEngine.Input.GetButtonUp("Jump"),
            X = UnityEngine.Input.GetAxisRaw("Horizontal"),

            PrimaryFire = isFire1,
        };
    }
}
