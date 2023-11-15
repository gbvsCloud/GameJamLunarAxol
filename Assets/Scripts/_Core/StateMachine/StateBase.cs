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
    public virtual void FixedUpdate()
    {
    }
    public virtual void CheckStateSwitch()
    {
    }
}


    