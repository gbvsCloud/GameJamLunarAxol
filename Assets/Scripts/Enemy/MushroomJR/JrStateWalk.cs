using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JrStateWalk : StateBase
{
    Mushroom enemy;
    Animator animator;
    public override void OnStateEnter(params object[] objs)
    {
        enemy = objs[0] as Mushroom;
        animator = enemy.GetComponent<Animator>();
        animator.SetTrigger("Walk");
    }
    public override void OnStateExit()
    {
        animator.ResetTrigger("Walk");
    }
    public override void FixedUpdate()
    {
        enemy.Patrol();
    }
    public override void CheckStateSwitch()
    {
    }
}
