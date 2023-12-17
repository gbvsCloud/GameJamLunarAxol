using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Passage : MonoBehaviour
{
    public string sceneToLoad;
    public string passageExit;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            LevelManager.passageExit = passageExit;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
