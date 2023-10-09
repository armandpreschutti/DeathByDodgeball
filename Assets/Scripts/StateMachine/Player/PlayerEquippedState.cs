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

    public override void ExitState()
    {
        Ctx.IsEquipped = false;
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
