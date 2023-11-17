using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkState : EnemyState
{
    public EnemyWalkState(EnemyStateMachine stateMachine, Enemy enemy) : base(stateMachine, enemy)
    {

    }

    public override void OnStateStay()
    {
       // Debug.Log("ANDANDO");
    }
}
