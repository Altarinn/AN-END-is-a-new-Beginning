using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FrameInput : IEquatable<FrameInput>
{
    public float X, Y;
    public bool JumpDown;
    public bool JumpUp;

    public bool PrimaryFire;

    public bool beingHit; // Animation trigger (placeholder)

    // We won't handle each input channel separately for easy development.
    public static bool operator ==(FrameInput obj1, FrameInput obj2)
    {
        if (ReferenceEquals(obj1, obj2))
            return true;
        if (ReferenceEquals(obj1, null))
            return false;
        if (ReferenceEquals(obj2, null))
            return false;
        return obj1.Equals(obj2);
    }
    public static bool operator !=(FrameInput obj1, FrameInput obj2) => !(obj1 == obj2);
    public bool Equals(FrameInput other)
    {
        if (ReferenceEquals(other, null))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        
        return 
            X.Equals(other.X) && Y.Equals(other.Y)
         && JumpDown.Equals(other.JumpDown)
         && JumpUp.Equals(other.JumpUp)
         && PrimaryFire.Equals(other.PrimaryFire)
         && beingHit.Equals(other.beingHit);
    }
    public override bool Equals(object obj) => obj is FrameInput other && this.Equals(other);

    public override int GetHashCode()
    {
        return (X, Y, JumpDown, JumpUp, PrimaryFire, beingHit).GetHashCode();
    }
}

public class ReplayableInput : MonoBehaviour
{
    public FrameInput Input { get; private set; }

    float fireCounter;

    [SerializeField]
    float fireIntervalMS = 100.0f;

    public class InputRecord
    {
        public struct RecordEntry
        {
            public FrameInput input;

            //int framesSustained;
            public float timeSustained;

            // Position right after this entry finshed.
            public Vector2 referencePos;
        }

        public List<RecordEntry> records = new();
        public float totalTime { get; private set; } = 0;
        public Vector2 initialPosition;

        public void AddRecordEntry(RecordEntry entry)
        {
            totalTime += entry.timeSustained;
            records.Add(entry);
        }

        public void Clear()
        {
            records.Clear();
            totalTime = 0;
            initialPosition = Vector2.zero;
        }
    }

    private void Awake()
    {
        fireCounter = 0.0f;
    }

    // Update is called once per frame
    bool inputEnabled = false;
    public bool InputEnabled { 
        get { return inputEnabled; }
        set 
        { 
            inputEnabled = value;

            if(value == false)
            {
                Input = default(FrameInput);
            }
        }
    }
    void Update()
    {
        GatherInput();
        RecordUpdate();
    }

    private void GatherInput()
    {
        if(state == RecorderState.Replay)
        {
            Input = UpdateReplay();
        }

        // Record or Idle
        else if(InputEnabled)
        {
            // Handle sustained input as multiple repeated inputs
            bool isFire1 = false;

            // TODO: Change to fixed 60FPS and int fireCounter?
            if (fireCounter >= 0) { fireCounter -= Time.deltaTime; }

            if (fireCounter < 0 && UnityEngine.Input.GetButton("Fire1"))
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

    #region Recording
    public enum RecorderState
    {
        Idle,
        Record,
        Replay
    }
    public RecorderState state { get; protected set; } = RecorderState.Idle;
    InputRecord recordInRecording;

    FrameInput lastAction;
    float timeSinceLastEntry = 0.0f;

    public void SetState(RecorderState newState)
    {
        state = newState;
    }

    private void StartRecording(InputRecord record)
    {
        if (state != RecorderState.Idle)
        {
            Debug.LogError("Previous record / replay has not finished yet!");
            return;
        }

        record.Clear();
        recordInRecording = record;
        state = RecorderState.Record;

        recordInRecording.initialPosition = transform.position;

        timeSinceLastEntry = 0.0f;
    }

    private void EndRecording()
    {
        if (state != RecorderState.Record)
        {
            Debug.LogError("Not in record mode, cannot stop!");
            return;
        }

        recordInRecording.AddRecordEntry(new InputRecord.RecordEntry()
            {
                input = lastAction,
                timeSustained = timeSinceLastEntry,
                referencePos = transform.position
            }
        );
        timeSinceLastEntry = 0.0f;

        state = RecorderState.Idle;
        recordInRecording = null;
    }

    private void RecordUpdate()
    {
        if(state == RecorderState.Record && recordInRecording != null)
        {
            if(lastAction == null)
            {
                lastAction = Input;
            }
            else if(lastAction != Input)
            {
                recordInRecording.AddRecordEntry(new InputRecord.RecordEntry()
                    { 
                        input = lastAction,
                        timeSustained = timeSinceLastEntry,
                        referencePos = transform.position
                    }
                );

                timeSinceLastEntry = 0.0f;
                lastAction = Input;
            }

            timeSinceLastEntry += Time.deltaTime;
        }
    }
    #endregion

    #region Replay

    InputRecord recordInReplay;
    int currentEntryIndex = 0;

    private void StartReplay(InputRecord record)
    {
        if (state != RecorderState.Idle)
        {
            Debug.LogError("Previous record / replay has not finished yet!");
            return;
        }

        state = RecorderState.Replay;
        recordInReplay = record;

        // TODO: Directly use record's position like this?
        transform.position = new Vector3(record.initialPosition.x, record.initialPosition.y, transform.position.z);

        timeSinceLastEntry = 0;
        currentEntryIndex = 0;
    }

    private void EndReplay()
    {
        if (state != RecorderState.Replay)
        {
            Debug.LogError("Not in replay mode, cannot stop!");
            return;
        }

        state = RecorderState.Idle;
        recordInReplay = null;

        timeSinceLastEntry = 0;
        currentEntryIndex = 0;
    }

    private FrameInput UpdateReplay()
    {
        if (state != RecorderState.Replay)
        {
            Debug.LogError("Not in replay mode, cannot get replay input!");
            return Input;
        }

        InputRecord.RecordEntry entry = recordInReplay.records[currentEntryIndex];

        timeSinceLastEntry += Time.deltaTime;
        if(timeSinceLastEntry >= entry.timeSustained)
        {
            transform.position = new Vector3(entry.referencePos.x, entry.referencePos.y, transform.position.z);

            //timeSinceLastEntry -= entry.timeSustained;
            timeSinceLastEntry = 0;
            currentEntryIndex++;
        }

        // Finish?
        if(currentEntryIndex >= recordInReplay.records.Count)
        {
            EndReplay();
        }

        return entry.input;
    }
    #endregion

    #region Debug area

    public bool ShowDebugMenu = false;
    InputRecord testRecord = new InputRecord();

    private void OnGUI()
    {
        if(ShowDebugMenu)
        {
            GUI.skin = DebugUI.Instance.debugUISkin;

            GUILayout.BeginArea(new Rect(0, 80, 160, 500));

            if (state == RecorderState.Replay)
            {
                GUILayout.Label($"Input state: {state}\nRecord: {currentEntryIndex} / {testRecord.records.Count}");
            }
            else
            {
                GUILayout.Label($"Input state: {state}\nRecord length: {testRecord.records.Count}");
            }

            if (GUILayout.Button("Start record"))
            {
                StartRecording(testRecord);
            }

            if (GUILayout.Button("Stop  record"))
            {
                EndRecording();
            }

            if (GUILayout.Button("Start replay"))
            {
                StartReplay(testRecord);
            }

            if (GUILayout.Button("Stop  replay"))
            {
                EndReplay();
            }

            GUILayout.EndArea();
        }
    }

    #endregion
}
