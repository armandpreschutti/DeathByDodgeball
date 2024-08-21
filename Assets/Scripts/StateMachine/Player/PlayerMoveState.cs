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

    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.CurrentSubState = "Move State";

        Ctx.transform.Translate(Ctx.MoveDirection * Ctx.MoveSpeed * Time.deltaTime);
        if (Ctx.MoveDirection.magnitude > .65 && !Ctx.IsThrowing)
        {
            bool flipped = Ctx.MoveDirection.x < 0f;

            Ctx.GetComponent<SpriteRenderer>().flipX = flipped;
        }
        if (Ctx.IsEquipped)
        {
            Ctx.ChangeBallPosition(Ctx.EquippedBall);
        }

    }

    public override void FixedUpdateState()
    {
       
    }

    public override void ExitState()
    {

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
