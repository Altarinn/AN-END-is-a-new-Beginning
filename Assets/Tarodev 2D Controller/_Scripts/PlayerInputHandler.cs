using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerInputHandler : MonoBehaviour
{
    public ReplayableInput InputModule;

    // This is horrible, but for some reason colliders are not fully established when update starts...
    protected bool _active;

    protected virtual void Awake() => Invoke(nameof(Activate), 0.5f);

    protected virtual void Activate()
    {
        if (InputModule == null)
        {
            Debug.LogError($"{name} has no ReplayableInput assigned for PlayerController!");
            return;
        }

        _active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_active) return;
        FrameInput input = GatherInput();

        HandleInput(input);
    }

    protected abstract void HandleInput(FrameInput input);

    private FrameInput GatherInput()
    {
        return InputModule.Input;
    }
}
