using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EntityBase
{
    [SerializeField]
    private Transform patrolPositionA;
    [SerializeField]
    private Transform patrolPositionB;
    
    private Transform currentPatrolPosition;

    [SerializeField]
    private Rigidbody2D rigidBody;

    float speed = 4;

    void Start()
    {

        Init(3);

        currentPatrolPosition = patrolPositionA;
        patrolPositionA.transform.position = new Vector2(patrolPositionA.transform.position.x, transform.position.y);
        patrolPositionB.transform.position = new Vector2(patrolPositionB.transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Patrol();
    }

    public void Patrol()
    {
        if (currentPatrolPosition == patrolPositionA)
        {
            rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);

        }
        else
        {
            rigidBody.velocity = new Vector2(-speed, rigidBody.velocity.y);
        }

        if (Vector2.Distance(transform.position, currentPatrolPosition.position) < 0.5f && currentPatrolPosition == patrolPositionB)
        {
            currentPatrolPosition = patrolPositionA;
        }
        if (Vector2.Distance(transform.position, currentPatrolPosition.position) < 0.5f && currentPatrolPosition == patrolPositionA)
        {
            currentPatrolPosition = patrolPositionB;
        }

    }



}
