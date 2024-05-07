using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExecutionState
{
    None,
    Active,
    Completed,
    Terminated
}

public enum FSMStateType
{
    Idle,
    Patrol,
    Chase,    
    Stomp, 
    Charge
}

public abstract class AbstractFSMState : ScriptableObject
{
    public ExecutionState ExecutionState{ get; protected set; }
    protected FSM fsm;
    public FSMStateType stateType;

    public virtual void OnEnable()
    {
        ExecutionState = ExecutionState.None;
    }

    public virtual bool EnterState()
    {
        ExecutionState = ExecutionState.Active;
        return true;
    }

    public abstract void UpdateLogic();
    
    public virtual bool ExitState()
    {
        ExecutionState = ExecutionState.Completed;
        return true;
    }

    public void SetFSM(FSM machine)
    {
        fsm = machine;
    }
}
