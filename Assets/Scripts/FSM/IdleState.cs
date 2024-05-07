using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/IdleState")]
public class IdleState : AbstractFSMState
{

    public override void OnEnable()
    {
        base.OnEnable();
        stateType = FSMStateType.Idle;

        //Should probably create an init function;
    }

    public override bool EnterState()
    {
        base.EnterState();
        Debug.Log("EnterIdle");
        return true;
    }

    public override void UpdateLogic()
    {
        
    }

    public override bool ExitState()
    {
        base.ExitState();
        Debug.Log("ExitIdle");
        return true;
    }
}