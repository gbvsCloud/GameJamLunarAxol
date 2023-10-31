using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : Singleton<StateMachine>
{
    public enum States
    {
        IDLE,
        RUNNING,
        DEAD,
        SWING,
        JUMPING
    }
    public Dictionary<States, StateBase> dictionaryState;
    private StateBase _currentState;
    public Player player;
    public TongueManager manager;

    public override void Awake()
    {
        base.Awake();

        dictionaryState = new Dictionary<States, StateBase>();
        RegisterStates(States.IDLE, new StateIdle());
        RegisterStates(States.RUNNING, new StateRun());
        RegisterStates(States.DEAD, new StateDead());
        RegisterStates(States.SWING, new StateSwing());
        RegisterStates(States.JUMPING, new StateJump());

        SwitchState(States.IDLE, manager, player);
    }
    public void RegisterStates(States typeEnum, StateBase state)
    {
        dictionaryState.Add(typeEnum, state);
    }
    public void SwitchState(States state, params object[] objs)
    {
        if (dictionaryState[state].Equals(_currentState)) return;

        if (_currentState != null) _currentState.OnStateExit();
        _currentState = dictionaryState[state];

        if (_currentState != null) _currentState.OnStateEnter(objs);
    }
    public void Update()
    {
        if (_currentState != null) _currentState.OnStateStay();
        Debug.Log(_currentState);
    }
}