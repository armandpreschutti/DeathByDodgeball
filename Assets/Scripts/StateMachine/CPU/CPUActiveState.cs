using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUActiveState : CPUBaseState
{
    public CPUActiveState(CPUBrain currentContext, CPUStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
        InitializeSubState();

    }

    public override void EnterState()
    {
       // Debug.Log("CPU entered Active State");

    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.CurrentSuperState = "Active State";
    }

    public override void FixedUpdateState()
    {

    }


    public override void ExitState()
    {

    }

    public override void CheckSwitchState()
    {

    }

    public override void InitializeSubState()
    {
        SetSubState(Factory.Idle());
       // SwitchState(Factory.Idle());
    }
}
