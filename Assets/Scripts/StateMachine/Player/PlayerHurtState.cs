using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtState : PlayerBaseState
{


    public PlayerHurtState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) 
    {

    }

    public override void EnterState() 
    {
        Ctx.OnHurt?.Invoke();
       // Ctx.StartCoroutine(Hurt());
        Ctx.Anim.SetBool("Hit", true);
        Ctx.Anim.Play("Hit");
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.CurrentSubState = "Hurt State";
    }

    public override void FixedUpdateState()
    {

    }


    public override void ExitState() 
    {
        Ctx.Anim.SetBool("Hit", false);
    }

    public override void CheckSwitchState() 
    {
        if (Ctx.IsDead)
        {
            SwitchState(Factory.Death());
        }
    }

    public override void InitializeSubState() 
    {

    }

}
