using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class BigFrogStunned : StateBase
{
    public BigFrog boss;

    public override void OnStateEnter(params object[] objs)
    {
        boss = objs[0] as BigFrog;
        boss.Stunned();   
    }

    public override void OnStateStay()
    {
        
    }
    public override void OnStateExit()
    {

    }
    public override void CheckStateSwitch()
    {   
       
    }
}
