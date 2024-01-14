using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFrogJump : StateBase
{
    public BigFrog boss;
    public bool alreadyJumped = false;

     public override void OnStateEnter(params object[] objs)
    { 
        boss = objs[0] as BigFrog;   
        alreadyJumped = false;
        if(!boss.bossStunned){
            boss.LookAt(boss.player.transform);
            boss.JumpToPlayer();
            boss.jumpCount ++;
            boss.jumped = true;
            boss.isGrounded = false;
            alreadyJumped = true;
        }
    }

    public override void OnStateStay()
    {
        if(!boss.bossStunned && boss.isGrounded && !alreadyJumped){
            boss.LookAt(boss.player.transform);
            boss.JumpToPlayer();
            boss.jumpCount ++;
            boss.jumped = true;
            boss.isGrounded = false;
            alreadyJumped = true;
        }
        CheckStateSwitch();
        
    }
    public override void OnStateExit()
    {
    }
    public override void CheckStateSwitch()
    {   
        if(boss.isGrounded && boss.jumped)
            boss.stateMachine.SwitchState(BigFrog.States.GROUNDED, boss);
    }

}
   
