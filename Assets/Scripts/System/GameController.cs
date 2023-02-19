using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;
using DG.Tweening;

#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif
using UnityEngine.UI;

public class GameController : SingletonMonoBehaviour<GameController>
{
    [HideInInspector]
    public double logoutTime;

    [HideInInspector]
    public int offlineTime = 0;

    [Header("基础组件")]
    public GameObject MainMenu;
    public GameObject MainCamera;
    public GameObject DeathExplosion;

#if PLATFORM_ANDROID
    GameObject dialog = null;
#endif

    public static GameController GetInstance() => Instance;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);

        if (gameObject.activeSelf)
        {
            GetPermission();
            DataInit();
            //bool ifFirst = Save.GetInstance().FirstLog();
            //if (ifFirst)
            FirstInit();
            Init();
        }
    }

    void Start()
    {
        //GetPermission();
        //DataInit();
        //bool ifFirst = Save.GetInstance().FirstLog();
        //if (ifFirst)
        //    FirstInit();
        //Init();
    }

    private void DataInit()
    {
        //DataLoader.GetInstance().LoadData();
    }

    private void Init()
    {
        // Maybe we won't fix the frame rate ... ?
        // Application.targetFrameRate = 60;

        //DataLoader.GetInstance().InitSaveData();
        InitLevel("unspecified");
    }

    private void GetPermission()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
            dialog = new GameObject();
        }
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            dialog = new GameObject();
        }
#endif
    }

    void OnGUI()
    {
#if PLATFORM_ANDROID
        if(!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            return;
        }
        else if(!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            return;
        }
        else if(dialog != null)
        {
            Destroy(dialog);
        }
#endif

        if (currentRoom == null) { return; }

        if (DebugUI.Instance != null)
        {
            GUI.skin = DebugUI.Instance.debugUISkin;
        }

        GUI.Label(new Rect(400, 10, 100, 14), $"TIME: {currentRoom?.time}");

        if (GUI.Button(new Rect(400, 30, 100, 14), "GO Phantom"))
        {
            TEST_InstantPhantom();
        }

        if (GUI.Button(new Rect(400, 50, 100, 14), "Reset"))
        {
            RestartLevel();
        }
    }

    private void FirstInit()
    {
        GetPermission();
        //处理数据初始化

        //Save.GetInstance().SaveFunc();
    }

    private void OnDestroy()
    {
        //Save.GetInstance().SaveFunc();
    }

    public void ExitGame()
    {
        //Save.GetInstance().SaveFunc();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#elif UNITY_ANDROID
        Application.Quit();
#endif
    }

    #region Game-specific

    [Header("AN END is a new Beginning!")]

    public GameObject playerPrefab;
    public GameObject phantomPrefab;

    public bool IsPhantom { get; private set; }

    public Dictionary<string, Room> rooms = new();
    Room currentRoom;
    string doorEntered;

    public Sprite doorSprite;

    [HideInInspector] public GameObject player { get; private set; }
    private float initialWait = 1.0f;

    public void EnterLevelAsync(string sceneName, string doorName = "unspecified")
    {
        // Check if scene exists
        int buildIdx = SceneUtility.GetBuildIndexByScenePath(sceneName);
        if (buildIdx < 0)
        {
            Debug.LogWarning($"Scene \"{sceneName}\" does not exist!");
            return;
        }

        // TODO: Black screen
        StartCoroutine(LoadLevelAsync(sceneName, doorName));
    }

    IEnumerator LoadLevelAsync(string sceneName, string doorName)
    {
        //if (player) { DontDestroyOnLoad(player); }
        ExitCutScene();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until finish
        while (!asyncLoad.isDone) { yield return null; }

        InitLevel(doorName);
    }

    public void InitLevel(string doorName)
    {
        MainCamera = GameObject.FindObjectOfType<Camera>().gameObject;
        LoadPlayer(doorName);
        UIManager.Instance.RefreshState();

        // Terminates if no player generated (no spawn point found)
        if (player == null)
        {
            currentRoom = null;
            return;
        }

        InitRoom(SceneManager.GetActiveScene().name);
    }

    public void InitRoom(string roomName)
    {
        if (rooms.ContainsKey(roomName))
        {
            currentRoom = rooms[roomName];
        }
        else
        {
            currentRoom = new Room(roomName);
            rooms.Add(roomName, currentRoom);
        }

        currentRoom.InitRoom(player);
    }

    public void OpenAllDoors()
    {
        var doors = FindObjectsOfType<Door>();
        foreach (var door in doors)
        {
            door.Open();
        }
    }

    public void FinishRoom(string roomName)
    {
        OpenAllDoors();
    }

    public void RefreshRoomEnemyList() => currentRoom?.RefreshEnemyList();

    public void ExitStage()
    {
        IsPhantom = false;
        rooms.Clear();
    }

    public void LoadPlayer(string spawnName = "unspecified")
    {
        // Find spawn point
        Debug.Log($"Find spawn point {spawnName}");
        var spawnPoints = GameObject.FindObjectsOfType<Door>();
        Door targetSpawn = null;
        foreach (var spawnPoint in spawnPoints)
        {
            // 1st priority: spawn point with specific name
            if (spawnPoint.name == spawnName)
            {
                targetSpawn = spawnPoint;
            }

            // 2nd priority: default spawn point
            if (targetSpawn == null && spawnPoint.isDefaultDoor)
            {
                targetSpawn = spawnPoint;
            }
        }

        // 3rd priority: 1st spawn point
        if (targetSpawn == null && spawnPoints.Length > 0) { targetSpawn = spawnPoints[0]; }

        // No spawnpoint, not a level
        if (targetSpawn == null)
        {
            Debug.Log($"No spawn point found in {SceneManager.GetActiveScene().name}");
            return;
        }

        // Found target spawn point, put our player there
        if (player == null)
        {
            if (IsPhantom)
            {
                Debug.Log($"Instantiate phantom at {SceneManager.GetActiveScene().name}");
                player = Instantiate(phantomPrefab);
                player.SetActive(false);
                OpenAllDoors();
            }
            else
            {
                Debug.Log($"Instantiate player at {SceneManager.GetActiveScene().name}");
                player = Instantiate(playerPrefab);
                player.SetActive(false);
                targetSpawn.Open();
            }
        }

        player.SetActive(true);
        player.transform.position = targetSpawn.spawnPivot.position;

        var pc = player.GetComponent<TarodevController.PlayerController>();
        pc.Gravity = false;
        pc.UseInput = false;

        doorEntered = targetSpawn.name;

        // Wait for everything loaded, especially preventing player fall through the ground
        Invoke(nameof(ActivatePlayer), initialWait);
    }

    private void Update()
    {
        currentRoom?.UpdateRoom();
    }

    public void CameraShake(float strength = 0.5f)
    {
        MainCamera.transform.DOShakePosition(0.5f, strength, 13, 1, false, true);
    }

    public void ChangeToPhantom(Phantom phantom)
    {
        // Already phantom
        if (IsPhantom) { return; }

        if (player != null)
        {
            var plri = player.GetComponent<ReplayableInput>();
            plri.InputEnabled = false;

            var phri = phantom.GetComponent<ReplayableInput>();
            phri.InputEnabled = true;
        }

        player = phantom.gameObject;
        player.layer = LayerMask.NameToLayer("Phantom");
        IsPhantom = true;

        OpenAllDoors();
        UIManager.Instance.RefreshState();
    }

    bool isInCutscene = false;

    public void EnterCutScene()
    {
        if (player != null)
        {
            var pi = player.GetComponent<ReplayableInput>();
            pi.InputEnabled = false;
        }

        isInCutscene = true;
    }

    public void ExitCutScene()
    {
        if (player != null)
        {
            var pi = player.GetComponent<ReplayableInput>();
            pi.InputEnabled = true;
        }

        isInCutscene = false;
    }

    private void TEST_InstantPhantom()
    {
        // End room
        currentRoom?.FinishRoom();
        IsPhantom = true;

        // Reload
        StartCoroutine(TEST_InstantPhantom_ReloadLevel());
    }

    IEnumerator TEST_InstantPhantom_ReloadLevel()
    {
        var playerPos = player.transform.position;

        yield return StartCoroutine(LoadLevelAsync(SceneManager.GetActiveScene().name, "NO"));

        player.transform.position = playerPos;
    }

    public void TimeUp()
    {
        if (isInCutscene) { return; }
        RestartLevel();
    }

    public void RestartLevel()
    {
        if (isInCutscene) { return; }

        if (IsPhantom)
        {
            EnterLevelAsync(SceneManager.GetActiveScene().name, doorEntered);
        }
        else
        {
            EnterLevelAsync(SceneManager.GetActiveScene().name);
        }
    }

    HashSet<int> ObtainedItems = new();
    int nBerries = 0;
    bool playerHasBomb = false;

    public bool ItemObtained(int id) => ObtainedItems.Contains(id);
    public void GetItem(int id)
    {
        ObtainedItems.Add(id);
    }

    public void GetBerry() => nBerries++;
    public void GetBomb() => playerHasBomb = true;

    void ActivatePlayer()
    {
        var pc = player.GetComponent<TarodevController.PlayerController>();
        pc.Gravity = true;
        pc.UseInput = true;
        pc.GetComponent<ReplayableInput>().InputEnabled = true;

        initialWait = 0;
    }

    #endregion

    #region DEBUG
    [Header("DEBUG")]
    public GUISkin debugUISkin;
    #endregion
}
