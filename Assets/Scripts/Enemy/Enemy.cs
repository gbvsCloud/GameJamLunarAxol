using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EntityBase
{
    private Transform currentPatrolPosition;

    [SerializeField]
    private Rigidbody2D rigidBody;

    float speed = 4;

    public bool goingRight = true;

    void Start()
    {

        Init(2);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Patrol();
    }

    public void Patrol()
    {
        if(goingRight)
        {
            rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);
        }
        else
        {
            rigidBody.velocity = new Vector2(-speed, rigidBody.velocity.y);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ReturnEnemy"))
        {
            goingRight = !goingRight;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "TornTiles")
        {
            Death();
        }
    }

}
