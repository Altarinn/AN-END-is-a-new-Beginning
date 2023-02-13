using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
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
    [HideInInspector] public GameObject player;

    public int livingEnemy = 0;

    private static LevelManager _instance = null;

    private LevelManager()
    {
        _instance = this;
    }

    public static LevelManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new LevelManager();
        }
        return _instance;
    }

    void Start()
    {
        Init();
    }

    private void Init()
    {
        //EnemyTarget = player.transform;
        enemyContainer = GameObject.Find("EnemyContainer").transform;
        bulletContainer = GameObject.Find("BulletsContainer").transform;
        areaEffectContainer = GameObject.Find("AreaEffectContainer").transform;

        DontDestroyOnLoad(this);
        DontDestroyOnLoad(enemyContainer);
        DontDestroyOnLoad(bulletContainer);
        DontDestroyOnLoad(areaEffectContainer);
    }

    public void EnterLevel(int sceneOrder)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneOrder);
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
