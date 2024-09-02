using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {

    }

    public override void EnterState()
    {
        Ctx.OnMove?.Invoke(true);
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.CurrentSubState = "Move State";

        Ctx.transform.Translate(Ctx.MoveDirection * Ctx.MoveSpeed * Time.deltaTime);
        Ctx.SetPlayerOrientation();
    }

    public override void FixedUpdateState()
    {
       
    }

    public override void ExitState()
    {
        Ctx.OnMove?.Invoke(false);
    }

    public override void CheckSwitchState()
    {
        if(Ctx.MoveInput == Vector2.zero)
        {
            SwitchState(Factory.Idle());
        }
        else if (Ctx.IsDodgePressed)
        {
            SwitchState(Factory.Dodge());
        }
        else if (Ctx.IsThrowPressed)
        {
            SwitchState(Factory.Aim());
        }
        else if (Ctx.IsCatchPressed)
        {
            SwitchState(Factory.Catch());
        }
        else if (Ctx.IsDead)
        {
            SwitchState(Factory.Death());
        }
    }

    public override void InitializeSubState()
    {

    }
}
