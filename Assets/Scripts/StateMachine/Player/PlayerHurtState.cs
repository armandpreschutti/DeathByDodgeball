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
        Ctx.Rb.velocity = new Vector2(Ctx.MoveDirection.x * 0f, Ctx.MoveDirection.y * 0f);
    }

    public override void ExitState() 
    {
        Ctx.Anim.SetBool("Hit", false);
    }

    public override void CheckSwitchState() 
    {
        if(Ctx.IsHurt == false)
        {
            SwitchState(Factory.Idle());
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
    public void Die()
    {
        Ctx.Anim.SetBool("Die", true);
        Ctx.UnequipBall(Ctx.EquippedBall);
        Ctx.IsDead = true;
    }
}
