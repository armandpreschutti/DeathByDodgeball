using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUDeathState : CPUBaseState
{
    public CPUDeathState(CPUBrain currentContext, CPUStateFactory cpuStateFactory) : base(currentContext, cpuStateFactory)
    {

    }

    public override void EnterState()
    {

    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.CurrentSubState = "Death State";
    }

    public override void FixedUpdateState()
    {

    }


    public override void ExitState()
    {
       
    }

    public override void CheckSwitchState()
    {

        if (!Ctx.playerStateMachine.IsDead)
        {
            SwitchState(Factory.Idle());
        }
    }

    public override void InitializeSubState()
    {

    }
}
