using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUIdleState : CPUBaseState
{

    public CPUIdleState(CPUBrain currentContext, CPUStateFactory cpuStateFactory) : base(currentContext, cpuStateFactory)
    {

    }

    public override void EnterState()
    {
        Debug.Log("CPU entered Idle State");

    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.CurrentSubState = "Idle State";
        Ctx.moveX = 0;
        Ctx.moveY = 0;
    }

    public override void FixedUpdateState()
    {

    }


    public override void ExitState()
    {

    }

    public override void CheckSwitchState()
    {
        if(!Ctx.playerStateMachine.IsEquipped)
        {
            SwitchState(Factory.BallSearching());
        }
    }

    public override void InitializeSubState()
    {

    }
}
