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

    }

    public override void FixedUpdateState()
    {
        Ctx.Rb.velocity = Ctx.MoveDirection * Ctx.MoveSpeed;
    }

    public override void ExitState()
    {
        Ctx.OnMove?.Invoke(false);
    }

    public override void CheckSwitchState()
    {
        if (Ctx.IsDead)
        {
            SwitchState(Factory.Death());
        }
        else
        {
            if (Ctx.MoveInput == Vector2.zero)
            {
                SwitchState(Factory.Idle());
            }
            else if (Ctx.IsDodgePressed && !Ctx.IsExhausted)
            {
                Ctx.DodgeDirection = Ctx.MoveDirection;
                SwitchState(Factory.Dodge());
            }
            else if (Ctx.IsThrowPressed)
            {
                SwitchState(Factory.Aim());
            }
            else if (Ctx.IsCatchPressed && Ctx.CanCatch)
            {
                SwitchState(Factory.Catch());
            }
        }
       
    }

    public override void InitializeSubState()
    {

    }
}
