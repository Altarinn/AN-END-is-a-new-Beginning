using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Room
{
    public string key;
    static HashSet<string> ignoredKeys = new HashSet<string>() { "Room1-F", "Room2-F", "Room3-F", "Room(Easter egg)" };

    public Room(string name)
    {
        key = name;
    }

    ReplayableInput.InputRecord playerRecord;
    GameController controller;
    ReplayableInput plri;

    public float time { get; private set; }
    bool roomCompleted;

    List<DamageTaker> enemies;
    List<EnemySpawn> spawns;

    public void InitRoom(GameObject player)
    {
        controller = GameController.Instance;

        if(controller.IsPhantom)
        {
            // Play replay if any
            if (playerRecord != null)
            {
                time = Mathf.Max(10.0f, playerRecord.totalTime);
                
                var plGhost = CreateGhostPlayer();
                plri = plGhost.GetComponent<ReplayableInput>();
                plri.StartReplay(playerRecord);
                plGhost.SetActive(true);
            }
            else
            {
                time = 99.0f;
            }
        }
        else
        {
            // Find all enemies
            RefreshEnemyList();

            enemies.ForEach(e => Debug.Log(e.gameObject));
            spawns.ForEach(e => Debug.Log(e.gameObject));

            // Keep a record if room not completed
            if (!roomCompleted)
            {
                time = 0;
                plri = player.GetComponent<ReplayableInput>();
                if (plri != null)
                {
                    playerRecord = new ReplayableInput.InputRecord();
                    plri.StartRecording(playerRecord);
                }
            }
            else
            {
                controller.FinishRoom(key);
                enemies.ForEach(e => e.gameObject.SetActive(false));
                spawns.ForEach(s => s.gameObject.SetActive(false));
            }
        }
    }

    public GameObject CreateGhostPlayer()
    {
        Debug.Log($"Instantiate player ghost at {SceneManager.GetActiveScene().name}");
        var player = GameObject.Instantiate(controller.playerPrefab);
        player.SetActive(false);

        return player;
    }

    public void RefreshEnemyList()
    {
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        enemies = GameObject.FindObjectsOfType<DamageTaker>().Where(dt => dt.gameObject.layer == enemyLayer && !dt.IsCrystal).ToList();
        spawns = GameObject.FindObjectsOfType<EnemySpawn>().ToList();
    }

    public bool CheckRoomCompleted()
        => controller.IsPhantom || (enemies.All(e => e.dead) && spawns.All(s => s.spawnFinished));

    public void UpdateRoom()
    {
        // Not initialized
        if(controller == null) { return; }

        if(controller.IsPhantom)
        {
            time -= Time.deltaTime;
            if(time < 0) { controller.TimeUp(); }
        }
        else if(!roomCompleted)
        {
            time += Time.deltaTime;
        }

        if (!roomCompleted && CheckRoomCompleted()) { FinishRoom(); }
    }

    public void FinishRoom()
    {
        if(!controller.IsPhantom)
        {
            roomCompleted = true;
            plri.EndRecording();
            controller.FinishRoom(key);
        }
    }
}
