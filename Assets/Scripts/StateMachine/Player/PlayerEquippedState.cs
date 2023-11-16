using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerManager;

public class PlayerEquippedState : PlayerBaseState
{
    public PlayerEquippedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) 
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    {
        Ctx.IsEquipped= true;
    }

    public override void UpdateState()
    {
        CheckSwitchState();
       
    }

    public override void FixedUpdateState()
    {
        if (!Ctx.IsAiming &&  !Ctx.IsThrowing && Ctx.MoveDirection.magnitude > .65f)
        {
            bool flipped = Ctx.MoveDirection.x < 0f;
            Ctx.HoldPosition = flipped ? Ctx.HoldRightPosition : Ctx.HoldLeftPosition;
            Ctx.ChangeBallPosition(Ctx.EquippedBall);
        }       
    }


    public override void ExitState()
    {
        Ctx.IsEquipped = false;
        Ctx.HoldPosition = null;
    }

    public override void CheckSwitchState() 
    {
        if (!Ctx.IsEquipped)
        {
            SwitchState(Factory.Unequipped());
        }
    }

    public override void InitializeSubState()
    {
        SetSubState(Factory.Idle());
    }    
}
