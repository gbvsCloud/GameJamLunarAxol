using System.Collections;
using UnityEngine;

public class JrStateIdle : StateBase
{
    Mushroom enemy;
    Animator animator;
    public override void OnStateEnter(params object[] objs)
    {
        enemy = objs[0] as Mushroom;
        animator = enemy.GetComponent<Animator>();
        animator.SetTrigger("Idle");
        enemy.rigidBody.velocity = Vector2.zero;
        enemy.StartIdleDuration();
    }

    
    public override void OnStateExit()
    {
        animator.ResetTrigger("Idle");
    }
    public override void FixedUpdate()
    {
    }
    public override void CheckStateSwitch()
    {
    }

}
