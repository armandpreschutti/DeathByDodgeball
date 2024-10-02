using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUCatchingState : CPUBaseState
{

    public CPUCatchingState(CPUBrain currentContext, CPUStateFactory cpuStateFactory) : base(currentContext, cpuStateFactory)
    {

    }

    public override void EnterState()
    {
        Ctx.playerStateMachine.IsCatchPressed = true;
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.CurrentSubState = "Catching State";
    }

    public override void FixedUpdateState()
    {

    }


    public override void ExitState()
    {
        Ctx.playerStateMachine.IsCatchPressed = false;
        Ctx.moveY = 0;
        Ctx.moveX = 0;
    }

    public override void CheckSwitchState()
    {
        if(!Ctx.playerStateMachine.IsDead)
        {
            if (!Ctx.playerStateMachine.IsCatching)
            {
                if (Ctx.playerStateMachine.IsEquipped)
                {
                    SwitchState(Factory.EnemySearching());
                }
                else
                {
                    SwitchState(Factory.Idle());
                }
            }
        }
        else
        {
            SwitchState(Factory.Death());
        }

    }

    public override void InitializeSubState()
    {

    }
}
