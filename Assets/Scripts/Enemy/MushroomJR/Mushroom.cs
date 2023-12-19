using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mushroom : Enemy
{
    IEnumerator stunI;
    Animator animator;

    public void Awake()
    {

        animator = GetComponent<Animator>();

        stateMachine = new StateMachine<States>();
        stateMachine.Init();
        stateMachine.RegisterStates(States.IDLE, new JrStateIdle());
        stateMachine.RegisterStates(States.WALKING, new JrStateWalk());


        stateMachine.SwitchState(States.WALKING, this);
    }

    public override void Knockback(Transform knockbackOrigin, float strength)
    { 
        if(stunI != null) StopCoroutine(stunI);
        stunI = StunDuration();
        StartCoroutine(stunI);

    }
    public void Patrol()
    {
        if(stunned){
            rigidBody.velocity = Vector2.zero;
            return; 
        } 
        
        if(goingRight)
        {
            rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);
        }
        else
        {
            rigidBody.velocity = new Vector2(-speed, rigidBody.velocity.y);
        }
    }

    public override void TurnAround()
    {
        stateMachine.SwitchState(States.IDLE, this);
    }

    public void StartIdleDuration(){
        StartCoroutine(IdleDuration());
    }
    IEnumerator IdleDuration(){
        yield return new WaitForSeconds(1f);
        stateMachine.SwitchState(Enemy.States.WALKING, this);
    }
    void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }
    IEnumerator StunDuration(){
        stunned = true;
        yield return new WaitForSeconds(1f);
        stunned = false;
    }

}
