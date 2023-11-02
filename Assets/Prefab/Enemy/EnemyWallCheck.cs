using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWallCheck : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    bool goingRight;
    float distanceCheck = 1f;
    private void Start()
    {
        goingRight = !enemy.goingRight;
    }

    private void Update()
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Map"))
        {
            enemy.goingRight = !enemy.goingRight;
            
        }
    }

}
