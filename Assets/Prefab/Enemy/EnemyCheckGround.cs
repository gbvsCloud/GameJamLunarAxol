using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCheckGround : MonoBehaviour
{
    [SerializeField] private Enemy enemy;

    bool goingRight;
    float distanceCheck = 1f;

    public bool groundAhead = false;
    public bool plataformAhead = false;

    private void Awake()
    {
        goingRight = !enemy.goingRight;
    }

    private void LateUpdate()
    {
        if (enemy.goingRight && goingRight != enemy.goingRight)
        {
            transform.position = new Vector2(enemy.transform.position.x + distanceCheck, transform.position.y);
            goingRight = enemy.goingRight;
        }
        else if (!enemy.goingRight && goingRight != enemy.goingRight)
        {
            transform.position = new Vector2(enemy.transform.position.x - distanceCheck, transform.position.y);
            goingRight = enemy.goingRight;
        }



    }


    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(!groundAhead && !plataformAhead){
            enemy.goingRight = !enemy.goingRight;
            if (enemy.goingRight)
            {
                transform.position = new Vector2(enemy.transform.position.x + distanceCheck, transform.position.y);
            }
            else
            {
                transform.position = new Vector2(enemy.transform.position.x - distanceCheck, transform.position.y);
            }
            enemy.TurnAround();
        }
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Map")){
            groundAhead  = true;
        }
        if(other.CompareTag("Plataforms")){
            plataformAhead = true;
        }
    }
    



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Map"))
        {      
            groundAhead = false;
        }

        if(collision.CompareTag("Plataforms")){
            plataformAhead = false;
        }
    }


}
