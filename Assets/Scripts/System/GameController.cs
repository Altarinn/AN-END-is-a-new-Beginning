using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if PLATFORM_ANDROID
using UnityEngine.Android;
# endif
using UnityEngine.UI;

public class GameController : MonoBehaviour
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
    private static GameController _instance = null;

    private GameController()
    {
        _instance = this;
    }

    public static GameController GetInstance()
    {
        if (_instance == null)
        {
            _instance = new GameController();
        }
        return _instance;
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        GetPermission();
        DataInit();
        bool ifFirst = Save.GetInstance().FirstLog();
        if (ifFirst)
            FirstInit();
        Init();
    }

    private void DataInit()
    {
        DataLoader.GetInstance().LoadData();
    }

    private void Init()
    {
        // Maybe we won't fix the frame rate ... ?
        //Application.targetFrameRate = 60;

        DataLoader.GetInstance().InitSaveData();
        LoadPlayer();
    }

    private void LoadPlayer()
    {

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

    #region DEBUG
    [Header("DEBUG")]
    public GUISkin debugUISkin;
    #endregion
}
