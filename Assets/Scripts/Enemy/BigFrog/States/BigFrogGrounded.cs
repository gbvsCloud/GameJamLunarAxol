using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFrogGrounded : StateBase
{
    public BigFrog boss;

    public override void OnStateEnter(params object[] objs)
    {
        boss = objs[0] as BigFrog;
        boss.jumped = false;
        boss.LookAt(boss.player.transform);    
        if(boss.jumpCount < 5){              
            boss.CreateWaves();
            boss.Jump();
        }
        CheckStateSwitch();
    }

    public override void OnStateStay()
    {
        
    }
    public override void OnStateExit()
    {

    }
    public override void CheckStateSwitch()
    {   
       if(boss.jumpCount >= 5){
            boss.stateMachine.SwitchState(BigFrog.States.STUNNED, boss);
        }
        
    }

}
