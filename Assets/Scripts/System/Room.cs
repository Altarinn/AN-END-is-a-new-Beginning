using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Room
{
    ReplayableInput.InputRecord playerRecord;
    GameController controller;
    ReplayableInput plri;

    public float time { get; private set; }
    bool roomCompleted;

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
            // Keep a record if room not completed
            if(!roomCompleted)
            {
                plri = player.GetComponent<ReplayableInput>();
                if (plri != null)
                {
                    playerRecord = new ReplayableInput.InputRecord();
                    plri.StartRecording(playerRecord);

                    // TODO: Check if room completed or not
                }
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

    public void UpdateRoom()
    {
        // Not initialized
        if(controller == null) { return; }

        if(controller.IsPhantom)
        {
            time -= Time.deltaTime;
        }
        else
        {
            time += Time.deltaTime;
        }
    }

    public void FinishRoom()
    {
        if(!controller.IsPhantom)
        {
            roomCompleted = true;
            plri.EndRecording();
        }
    }
}
