using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Room
{
    string key;

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
                time = playerRecord.totalTime;
                
                var plGhost = CreateGhostPlayer();
                plri = plGhost.GetComponent<ReplayableInput>();
                plri.StartReplay(playerRecord);
                plGhost.SetActive(true);
            }
        }
        else
        {
            // Find all enemies
            int enemyLayer = LayerMask.NameToLayer("Enemy");
            enemies = GameObject.FindObjectsOfType<DamageTaker>().Where(dt => dt.gameObject.layer == enemyLayer).ToList();
            spawns = GameObject.FindObjectsOfType<EnemySpawn>().ToList();

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

    public bool CheckRoomCompleted()
        => enemies.All(e => e.dead) && spawns.All(s => s.spawnFinished);

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
