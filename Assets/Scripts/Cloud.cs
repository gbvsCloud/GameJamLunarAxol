using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private Vector2 startPos;
    public float movementDuration;
    private float elapsedTime = 0;
    public float velocity;
    public bool goingRight;
    void Start()
    {
        startPos = transform.position;   
    }

    // Update is called once per frame
    void Update()
    {
        if(elapsedTime < movementDuration){
            if(goingRight){
                transform.position = new Vector2(transform.position.x + velocity * Time.deltaTime, transform.position.y);
            }else{
                transform.position = new Vector2(transform.position.x - velocity * Time.deltaTime, transform.position.y);
            }
            elapsedTime += Time.deltaTime;
        }else{
            elapsedTime = 0;
            transform.position = startPos;
        }
    }
}
