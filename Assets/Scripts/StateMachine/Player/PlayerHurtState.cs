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
        Ctx.Rb.velocity = Vector2.zero;
        Ctx.StartCoroutine(Hurt());
        Ctx.Anim.SetBool("Hit", true);
        Ctx.Anim.Play("Hit");
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
        Ctx.Anim.SetBool("Hit", false);
        Ctx.Rb.velocity = Vector2.zero;
    }

    public override void CheckSwitchState() 
    {
      /*  if(!Ctx.IsHurt)
        {
            SwitchState(Factory.Idle());
        }*/
        if (Ctx.IsDead)
        {
            SwitchState(Factory.Death());
        }
    }

    public override void InitializeSubState() 
    {

    }

    IEnumerator Hurt()
    {
        yield return new WaitForSeconds(Ctx.HurtDuration);
        Ctx.IsHurt = false;
    }
}
