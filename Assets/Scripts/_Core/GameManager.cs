using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour        
{
    [SerializeField]
    private GameObject pauseGroup;

    bool gamePaused = false;

    private void Start()
    {
        
    }

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


}
