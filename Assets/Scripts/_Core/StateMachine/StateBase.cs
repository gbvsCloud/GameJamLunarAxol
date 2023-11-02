using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StateBase
{
    public virtual void OnStateEnter(params object[] objs)
    {
    }
    public virtual void OnStateStay()
    {
    }
    public virtual void OnStateExit()
    {
    }
}
public class StateSwing : StateBase
{
    private TriggerSwing trigger;
    private Player player;
    public override void OnStateEnter(params object[] objs)
    {
        trigger = (TriggerSwing)objs[0];
        player = (Player)objs[1];
        player.GetComponent<PlayerMovement>().canRun = false;
        trigger.TongueAnimationStart();
        player.GetComponent<Animator>().SetTrigger("Swing");
    }
    public override void OnStateStay()
    {
        trigger.MotionBetween();
    }
    public override void OnStateExit()
    {
        trigger.SetIndex(0);
        player.GetComponent<PlayerMovement>().canRun = true;
    }
}


    