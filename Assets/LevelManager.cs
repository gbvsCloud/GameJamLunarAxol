using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public static string passageExit;   
    private GameObject player;
    public GameObject[] checkPoints;
    public override void Awake()
    {
        player = FindObjectOfType<Player>().gameObject;
        checkPoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        foreach(GameObject checkpoint in checkPoints){
            Checkpoint checkpointData = checkpoint.GetComponent<Checkpoint>();
            if(checkpointData.checkPointName == passageExit){
                player.transform.position = checkpoint.transform.position;
                if(!checkpointData.lookRight)
                    player.GetComponent<SpriteRenderer>().flipX = true;
            }
        }

    }

}
