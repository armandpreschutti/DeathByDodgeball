using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnequippedState : PlayerBaseState
{

    public PlayerUnequippedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {
        IsRootState= true;
        InitializeSubState();
    }

    public override void EnterState() 
    {


    }

    public override void UpdateState()
    {
        CheckSwitchState();
    }

    public override void FixedUpdateState()
    {

    }


    public override void ExitState() 
    {

    }

    public override void CheckSwitchState() 
    {
        if(Ctx.IsEquipped)
        {
            SwitchState(Factory.Equipped());
        }
    }

    public override void InitializeSubState()
    {
        SetSubState(Factory.Idle());
    }
}
