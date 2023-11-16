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

        if (Ctx.MoveDirection.magnitude > .65 && !Ctx.IsThrowing)
        {
            bool flipped = Ctx.MoveDirection.x < 0f;

            Ctx.GetComponent<SpriteRenderer>().flipX = flipped;
        }
        
    }

    public override void FixedUpdateState()
    { 
        Ctx.Rb.velocity = new Vector2(Ctx.MoveDirection.x * Ctx.IdleSpeed, Ctx.MoveDirection.y * Ctx.IdleSpeed);
    }

    public override void ExitState()
    {

    }

    public override void CheckSwitchState() 
    {
        if (Ctx.IsDodgePressed)
        {
            SwitchState(Factory.Dodge());
        }
        else if (Ctx.IsAimPressed)
        {
            SwitchState(Factory.Aim());
        }
        else if (Ctx.IsCatchPressed)
        {
            SwitchState(Factory.Catch());
        }
        else if (Ctx.IsHurt)
        {
            SwitchState(Factory.Hurt());
        }
        else
        {
            return;
        }
    }

    public override void InitializeSubState() 
    {

    }
}
