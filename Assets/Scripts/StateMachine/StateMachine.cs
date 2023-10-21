using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Core.StateMachine
{

    public class StateMachine : MonoBehaviour
    {

        public enum States
        {
            Idle,
            Run,
            Jump,
            SuperJump
        }

        public Dictionary<States, StateBase> dictionaryState;

        public StateBase _currentState;

        private void Start()
        {
            Init();
        }

        public StateBase CurrentState
        {
            get { return _currentState; }
        }

        public void Init()
        {
            dictionaryState = new Dictionary<States, StateBase>();
            RegisterStates(States.Idle, new Idle());
            SwitchState(States.Idle);

        }

        public void RegisterStates(States typeEnum, StateBase state)
        {
            dictionaryState.Add(typeEnum, state);
        }

        public void SwitchState(States state, params object[] objs)
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
        }
        
    }
}
