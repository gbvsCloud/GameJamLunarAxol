using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StateMachine<T> where T : System.Enum
{
    
    public Dictionary<T, StateBase> dictionaryState;
    private StateBase _currentState;

    public StateBase CurrentState
    {
        get { return _currentState; } 
    }   

    public void Init()
    {
        dictionaryState = new Dictionary<T, StateBase>();
    }
    public void RegisterStates(T typeEnum, StateBase state)
    {
        dictionaryState.Add(typeEnum, state);
    }
    public void SwitchState(T state, params object[] objs)
    {
        if (dictionaryState[state].Equals(_currentState))       
            return;       

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