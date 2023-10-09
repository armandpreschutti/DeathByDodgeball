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
        Ctx.IsCatching= true;
        Ctx.Anim.SetBool("IsCatching", true);
        Ctx.StartCoroutine(Catch());
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.Rb.velocity = new Vector2(Ctx.MoveDirection.x * 0f, Ctx.MoveDirection.y * 0f);
    }

    public override void ExitState() 
    {
        Ctx.IsCatching = false;
        Ctx.IsCatchPressed= false;
        Ctx.Anim.SetBool("IsCatching", false);
    }

    public override void CheckSwitchState() 
    {
        if (Ctx.IsCatching == false)
        {
            SwitchState(Factory.Idle());
        }
        else if(Ctx.IsCatchPressed == false)
        {
            SwitchState(Factory.Idle());
        }
        else if (Ctx.IsEquipped)
        {
            SwitchState(Factory.Idle());
        }
    }

    public override void InitializeSubState() 
    {

    }
    
    public IEnumerator Catch()
    {     
        yield return new WaitForSeconds(Ctx.CatchDuration);
        Ctx.IsCatching = false;
    }
}
