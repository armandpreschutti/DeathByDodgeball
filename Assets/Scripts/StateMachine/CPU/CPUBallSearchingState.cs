using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUBallSearchingState : CPUBaseState
{
    Coroutine testRoutine;

    public CPUBallSearchingState(CPUBrain currentContext, CPUStateFactory cpuStateFactory) : base(currentContext, cpuStateFactory)
    {

    }

    public override void EnterState()
    {

    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.CurrentSubState = "Searching State";
        SetVeritcalMovement();
        SetHorizontalMovement();
    }

    public override void FixedUpdateState()
    {

    }


    public override void ExitState()
    {

    }

    public override void CheckSwitchState()
    {
        if(Ctx.playerStateMachine.IsEquipped)
        {
            SwitchState(Factory.Idle());
        }
    }

    public override void InitializeSubState()
    {

    }
    public void SetHorizontalMovement()
    {
        if (Ctx.isFacingLeft)
        {
            if(Ctx.transform.position.x >= .5)
            {
                Ctx.moveX = -1;
            }
            else
            {
                Ctx.moveX = 0;
            }
        }
        else
        {
            if (Ctx.transform.position.x <= - .5)
            {
                Ctx.moveX = 1;
            }
            else
            {
                Ctx.moveX = 0;
            }
        }
    }

    public void SetVeritcalMovement()
    {
        if (Ctx.closestFreeBall != null)
        {
            if (Ctx.closestFreeBall.transform.position.y >= Ctx.transform.position.y +.3)
            {
                Ctx.moveY = 1;
            }
            else if (Ctx.closestFreeBall.transform.position.y <= Ctx.transform.position.y +.1)
            {
                Ctx.moveY = -1;
            }
            else
            {
                Ctx.moveY = 0;
            }
        }
    }
}
