using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFrogIdle : StateBase
{
    public BigFrog boss;

    public override void OnStateEnter(params object[] objs)
    {
        
        boss = objs[0] as BigFrog;   
        //boss?.GetComponent<Animator>().SetTrigger("Idle");
    }

    public override void OnStateStay()
    {
        boss.LookAt(boss.player.transform);
        CheckStateSwitch();
    }
    public override void OnStateExit()
    {
        //boss?.GetComponent<Animator>().ResetTrigger("Idle");
    }
    public override void CheckStateSwitch()
    {   
       
    }

}
