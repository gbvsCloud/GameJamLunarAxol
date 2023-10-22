using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private GameObject pauseGroup;

    [SerializeField]
    private List<Transform> checkpoints;

    [SerializeField]
    private Transform lastCheckpoint;

    [SerializeField]
    private Player player;

    bool gamePaused = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gamePaused)
            {
                gamePaused = true;
                Time.timeScale = 0;
                pauseGroup.SetActive(true);
            }
            else
            {
                gamePaused = false;
                Time.timeScale = 1;
                pauseGroup.SetActive(false);
            }
        }
    }

    public void Unpause()
    {
        if (gamePaused)
        {
            gamePaused = false;
            Time.timeScale = 1;
            pauseGroup.SetActive(false);
        }
    }

    public void NewCheckPoint(Transform checkpoint)
    {
        if(checkpoints.IndexOf(checkpoint) > checkpoints.IndexOf(lastCheckpoint))
        {
            lastCheckpoint = checkpoint;
        }

        
    }

    public void ReturnToLastCheckpoint()
    {
        player.transform.position = lastCheckpoint.position;
    }

}

