using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerInputHandler : MonoBehaviour
{
    public InputModule InputModule;

    // This is horrible, but for some reason colliders are not fully established when update starts...
    protected bool _active;

    protected virtual void Awake() => Invoke(nameof(Activate), 0.5f);

    protected virtual void Activate()
    {
        if (InputModule == null)
        {
            Debug.LogWarning($"{name} has no ReplayableInput assigned for PlayerController! Default inputs will be used ...");
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
        if(InputModule == null)
        {
            return default(FrameInput);
        }

        return InputModule.Input;
    }
}
