using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Core.StateMachine
{
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

    public class Idle : StateBase
    {
        public override void OnStateEnter(params object[] objs)
        {

        }
        public override void OnStateStay()
        {

        }
        public override void OnStateExit()
        {

        }
    }

}