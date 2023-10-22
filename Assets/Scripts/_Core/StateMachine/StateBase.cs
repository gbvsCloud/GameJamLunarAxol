using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StateBase
{
    public virtual void OnStateEnter(object o = null)
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
    public override void OnStateEnter(object o = null)
    {
        manager = (TongueManager)o;
    }
    public override void OnStateStay()
    {
        manager.TonguePosition();
    }
}
public class StateSwing : StateBase
{
    private TriggerSwing trigger;
    public override void OnStateEnter(object o = null)
    {
        trigger = (TriggerSwing)o;
        trigger.TongueAnimationStart();
    }
    public override void OnStateStay()
    {
        trigger.MotionBetween();
    }
    public override void OnStateExit()
    {
        trigger.SetIndex(0);
    }
}