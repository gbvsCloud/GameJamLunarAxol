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
        SWING
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
        RegisterStates(States.RUNNING, new StateBase());
        RegisterStates(States.DEAD, new StateBase());
        RegisterStates(States.SWING, new StateSwing());
        SwitchState(States.IDLE, manager);
    }
    public void RegisterStates(States typeEnum, StateBase state)
    {
        dictionaryState.Add(typeEnum, state);
    }
    public void SwitchState(States state, object o = null)
    {
        if (dictionaryState[state].Equals(_currentState))
            return;

        if (_currentState != null) _currentState.OnStateExit();
        _currentState = dictionaryState[state];

        if (_currentState != null) _currentState.OnStateEnter(o);
    }
    public void Update()
    {
        if (_currentState != null) _currentState.OnStateStay();
    }
}