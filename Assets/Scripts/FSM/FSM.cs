using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    [SerializeField] private AbstractFSMState _startingState;
    [SerializeField] private AbstractFSMState _currentState;

    [SerializeField] private List<AbstractFSMState> _states;
    private Dictionary<FSMStateType, AbstractFSMState> _fsmStates;

    public void Awake()
    {
        _currentState = null;
        _fsmStates = new Dictionary<FSMStateType, AbstractFSMState>();

        foreach (AbstractFSMState state in _states)
        {
            state.SetFSM(this);
            _fsmStates.Add(state.stateType, state);
        }
    }

    public void Start()
    {
        if (_startingState != null)
        {
            EnterState(_startingState);
        }
    }

    public void Update()
    {
        _currentState.UpdateLogic();
    }

    public void EnterState(AbstractFSMState nextState)
    {
        if (nextState == null)
        {
            return;
        }

        if (_currentState != null)
        {
            _currentState.ExitState();
        }

        _currentState = nextState;
        _currentState.EnterState();
    }

    public void EnterState(FSMStateType stateType)
    {
        if (_fsmStates.ContainsKey(stateType))
        {
            AbstractFSMState nextState = _fsmStates[stateType];
            EnterState(nextState);
        }
    }
}