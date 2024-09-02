using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowState : PlayerBaseState
{
    public PlayerThrowState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {

    }
    public override void EnterState()
    {
        Ctx.BaseAnim.SetBool("IsThrowing", true);
        Ctx.SkinAnim.SetBool("IsThrowing", true);
        Ctx.IsThrowing = true;
        Ctx.OnThrow?.Invoke(true);
       

    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.CurrentSubState = "Throw State";
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState()
    {

        Ctx.ThrowBall();
        Ctx.BaseAnim.SetBool("IsThrowing", false);
        Ctx.SkinAnim.SetBool("IsThrowing", false);
        Ctx.IsThrowing = false;
        Ctx.CurrentThrowPower = Ctx.MinThrowPower;
        Ctx.OnThrow?.Invoke(false);
    }

    public override void CheckSwitchState()
    {
        if (!Ctx.IsThrowing)
        {
            SwitchState(Factory.Idle());
        }
    }

    public override void InitializeSubState()
    {

    }
}
