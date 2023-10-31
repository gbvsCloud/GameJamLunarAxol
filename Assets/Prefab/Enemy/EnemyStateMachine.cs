using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    EnemyWalkState walkState;
    EnemyState currentState;

    Enemy enemy;

    public void Initialize()
    {
        walkState = new EnemyWalkState(this, enemy);

        ChangeState(walkState);
    }

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
    private void Update()
    {
        currentState.OnStateStay();
    }
    public void ChangeState(EnemyState state)
    {
        if (currentState != null) currentState.OnStateExit();
        currentState = state;
        currentState.OnStateEnter();


    }

}
