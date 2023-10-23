using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EntityBase
{
    private Transform currentPatrolPosition;

    [SerializeField]
    private Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    float speed = 4;

    public bool goingRight = true;

    void Start()
    {

        Init(1);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Patrol();
    }
    protected override void Update()
    {
        base.Update();
        if(goingRight)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
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
