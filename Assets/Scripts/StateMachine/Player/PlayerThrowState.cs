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
        Ctx.Anim.SetBool("IsThrowing", true);
        Ctx.IsThrowing = true;
        Ctx.OnThrow?.Invoke();
       

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
        //Ctx.EquippedBall.GetComponent<Rigidbody2D>().AddForce(Ctx.AimDirection * Ctx.CurrentThrowPower, ForceMode2D.Impulse
        Ctx.ThrowBall();
        Ctx.Anim.SetBool("IsThrowing", false);
        Ctx.IsThrowing = false;
       //Ctx.HoldPosition = null;
        Ctx.CurrentThrowPower = Ctx.MinThrowPower;

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
