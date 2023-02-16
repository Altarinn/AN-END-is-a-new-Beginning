using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;
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

#if PLATFORM_ANDROID
    GameObject dialog = null;
#endif

    public static GameController GetInstance() => Instance;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);

        if(gameObject.activeSelf)
        {
            GetPermission();
            DataInit();
            bool ifFirst = Save.GetInstance().FirstLog();
            if (ifFirst)
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
        LoadPlayer();
        InitRoom(SceneManager.GetActiveScene().name);
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

        GUI.skin = DebugUI.Instance.debugUISkin;
        GUI.Label(new Rect(400, 10, 100, 14), $"TIME: {currentRoom?.time}");
        
        if(GUI.Button(new Rect(400, 30, 100, 14), "GO Phantom"))
        {
            TEST_InstantPhantom();
        }
    }

    private void FirstInit()
    {
        GetPermission();
        //处理数据初始化

        Save.GetInstance().SaveFunc();
    }

    private void OnDestroy()
    {
        Save.GetInstance().SaveFunc();
    }

    public void ExitGame()
    {
        Save.GetInstance().SaveFunc();
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

    [HideInInspector] public GameObject player { get; private set; }
    private float initialWait = 1.0f;

    public void EnterLevelAsync(string sceneName, string doorName = "unspecified")
    {
        // Check if scene exists
        int buildIdx = SceneUtility.GetBuildIndexByScenePath(sceneName);
        if(buildIdx < 0)
        {
            Debug.LogWarning($"Scene \"{sceneName}\" does not exist!");
            return;
        }

        // TODO: Finish room properly
        if (currentRoom != null)
        {
            currentRoom.FinishRoom();
        }

        // TODO: Black screen
        Debug.Log($"Find spawn point {doorName}");
        StartCoroutine(LoadLevelAsync(sceneName, doorName));
    }

    IEnumerator LoadLevelAsync(string sceneName, string doorName)
    {
        //if (player) { DontDestroyOnLoad(player); }

        Debug.Log($"Find spawn point {doorName}");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until finish
        while (!asyncLoad.isDone) { yield return null; }

        LoadPlayer(doorName);
        InitRoom(SceneManager.GetActiveScene().name);
    }

    public void InitRoom(string roomName)
    {
        if(rooms.ContainsKey(roomName))
        {
            currentRoom = rooms[roomName];
        }
        else
        {
            currentRoom = new Room();
            rooms.Add(roomName, currentRoom);
        }

        currentRoom.InitRoom(player);
    }

    public void LoadPlayer(string spawnName = "unspecified")
    {
        if (player == null)
        {
            if(IsPhantom)
            {
                Debug.Log($"Instantiate phantom at {SceneManager.GetActiveScene().name}");
                player = Instantiate(phantomPrefab);
                player.SetActive(false);
            }
            else
            {
                Debug.Log($"Instantiate player at {SceneManager.GetActiveScene().name}");
                player = Instantiate(playerPrefab);
                player.SetActive(false);
            }
        }

        // Forget about it lol
        //SceneManager.MoveGameObjectToScene(player, SceneManager.GetActiveScene());

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

        // Found target spawn point, put our player there
        if (targetSpawn != null)
        {
            player.SetActive(true);
            player.transform.position = targetSpawn.spawnPivot.position;

            var pc = player.GetComponent<TarodevController.PlayerController>();
            pc.Gravity = false;
            pc.UseInput = false;

            // Wait for everything loaded, especially preventing player fall through the ground
            Invoke(nameof(ActivatePlayer), initialWait);
        }
    }

    private void Update()
    {
        currentRoom?.UpdateRoom();
    }

    public void ChangeToPhantom(Phantom phantom)
    {
        // Already phantom
        if (IsPhantom) { return; }

        if(player != null)
        {
            var plri = player.GetComponent<ReplayableInput>();
            plri.InputEnabled = false;

            var phri = phantom.GetComponent<ReplayableInput>();
            phri.InputEnabled = true;
        }

        player = phantom.gameObject;
        IsPhantom = true;
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
