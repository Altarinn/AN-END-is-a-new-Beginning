using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    public static Transform EnemyTarget;
    public static Transform enemyContainer;
    public static Transform bulletContainer;
    public static Transform areaEffectContainer;

    [HideInInspector] 
    public LevelState levelStateNow = LevelState.None;

    [HideInInspector]
    public int levelNow = 1;

    public GameObject playerPrefab;
    [HideInInspector] public GameObject player { get; private set; }

    public int livingEnemy = 0;

    public static LevelManager GetInstance() => Instance;


    void Start()
    {
        Init();
    }

    private void Init()
    {
        //EnemyTarget = player.transform;
        //enemyContainer = GameObject.Find("EnemyContainer").transform;
        //bulletContainer = GameObject.Find("BulletsContainer").transform;
        areaEffectContainer = GameObject.Find("AreaEffectContainer").transform;

        DontDestroyOnLoad(this);
        //DontDestroyOnLoad(enemyContainer);
        //DontDestroyOnLoad(bulletContainer);
        DontDestroyOnLoad(areaEffectContainer);
    }

    public void EnterLevel(int sceneOrder)
    {
        GameController.Instance.EnterLevelAsync("Scenes/Room(Easter egg)", "RightDoor");
    }

    public void EnterLevelStr(string scene)
    {
        GameController.Instance.EnterLevelAsync(scene);
        //SceneManager.LoadScene(scene);
    }

    public void Win()
    {
        levelStateNow = LevelState.Win;
    }

    public void Defeat(/*string enemyId*/)
    {
        levelStateNow = LevelState.Defeat;
    }

    public void ClearBattleField()
    {
        UsualTools.DeleteAllChildren(enemyContainer);
        UsualTools.DeleteAllChildren(bulletContainer);
        Destroy(player);
    }

    public enum LevelState
    {
        Win,
        Defeat,
        None,
    }
}
