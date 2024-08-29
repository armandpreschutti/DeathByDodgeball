using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCatchState : PlayerBaseState
{
    public PlayerCatchState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {

    }

    public override void EnterState()
    {
        Ctx.IsCatchPressed = false;
        Ctx.IsCatching = true;
        Ctx.Anim.SetBool("IsCatching", true);
        Ctx.OnCatch?.Invoke();
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.CurrentSubState = "Catch State";
    }

    public override void FixedUpdateState()
    {

    }

    public override void ExitState()
    {
        Ctx.Anim.SetBool("IsCatching", false);
        Ctx.IsCatching = false;
    }

    public override void CheckSwitchState()
    {
        if (Ctx.IsCatching == false)
        {
            SwitchState(Factory.Idle());
        }
        else if (Ctx.IsEquipped)
        {
            SwitchState(Factory.Idle());
        }
/*        else if(Ctx.IsDead)
        {
            SwitchState(Factory.Death());
        }*/
    }

    public override void InitializeSubState()
    {

    }
}