using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) 
    {

    }

    public override void EnterState()
    {

    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.CurrentSubState = "Idle State";
    }

    public override void FixedUpdateState()
    {
        Ctx.Rb.velocity = Vector2.zero;
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchState() 
    {
        if(Ctx.IsDead)
        {
            SwitchState(Factory.Death());
        }
        else
        {
            if (Ctx.MoveInput != Vector2.zero)
            {
                SwitchState(Factory.Move());
            }
            else if (Ctx.IsDodgePressed)
            {
                SwitchState(Factory.Dodge());
            }
            else if (Ctx.IsThrowPressed && Ctx.IsEquipped)
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
