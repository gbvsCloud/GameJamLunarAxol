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
public class StateIdle : StateBase
{
    private TongueManager manager;
    private Player player;
    public override void OnStateEnter(params object[] objs)
    {
        manager = (TongueManager)objs[0];
        player = (Player)objs[1];
        player.GetComponent<Animator>().SetTrigger("Idle");
    }
    public override void OnStateStay()
    {
        manager.TonguePosition();
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
        trigger.TongueAnimationStart();
        player.GetComponent<Animator>().SetTrigger("Swing");
        player.GetComponent<PlayerMovement>().canRun = false;
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

public class StateRun : StateBase
{
    private Player player;
    public override void OnStateEnter(params object[] objs)
    {
        player = (Player)objs[0];
        player.GetComponent<Animator>().SetBool("Run", true);
    }
    public override void OnStateExit()
    {
        player.GetComponent<Animator>().SetBool("Run", false);
    }
}