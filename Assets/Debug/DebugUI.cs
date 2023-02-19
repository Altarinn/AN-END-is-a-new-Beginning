using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DebugUI : SingletonMonoBehaviour<DebugUI>
{
    public GUISkin debugUISkin;

    public ReplayableInput[] inputs;
    int currentInputID = 0;
    public static DebugUI GetInstance() => Instance;

    protected override void Awake()
    {
        base.Awake();
        SetInputID(currentInputID);
    }

    bool SetInputID(int id)
    {
        return false;

        if(inputs.Any(input => 
            (input.state != ReplayableInput.RecorderState.Idle))
        )
        {
            return false;
        }

        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i].InputEnabled = false;
        }
        inputs[id].InputEnabled = true;

        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        return;

        GUI.skin = this.debugUISkin;

        GUILayout.BeginArea(new Rect(0, 140, 100, 500));

        GUILayout.Label($"Current: #{currentInputID} {inputs[currentInputID].gameObject.name}");
        if (GUILayout.Button("Next player"))
        {
            currentInputID++;
            currentInputID %= inputs.Length;

            if (!SetInputID(currentInputID))
            {
                currentInputID--;
                currentInputID %= inputs.Length;

                Debug.LogError("Can only switch player when all idle");
            }
        }

        GUILayout.EndArea();
    }
}
