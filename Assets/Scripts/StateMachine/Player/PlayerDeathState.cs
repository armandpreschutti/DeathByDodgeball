using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState: PlayerBaseState
{
    float stateTime;
    public PlayerDeathState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) 
    {

    }

    public override void EnterState()
    { 

        Ctx.DestroyBall();
        Ctx.BaseAnim.SetBool("Die", true);
        Ctx.SkinAnim.SetBool("Die", true);
        Ctx.OnDeath?.Invoke(true);
        Ctx.MoveInput = Vector2.zero;
        Ctx.IsAiming = false;
        Ctx.IsThrowPressed = false;
        Ctx.IsThrowing = false;
        Ctx.IsDodging= false;
        Ctx.IsCatching = false;
        Ctx.IsSuper = false;
        Ctx.Col.enabled = false;
        if (Ctx.CanRespawn)
        {
            Ctx.StartCoroutine(RespawnAfterDelay());
        }


    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.CurrentSuperState = "Death State";
        stateTime += Time.deltaTime;
        if(stateTime > .1)
        {
            Ctx.Rb.velocity = Vector2.zero;
        }
    }

    public override void FixedUpdateState()
    {
       
    }


    public override void ExitState() 
    {
        Ctx.Col.enabled = true;
        Ctx.BaseAnim.SetBool("Die", false);
        Ctx.SkinAnim.SetBool("Die", false);
        Ctx.OnDeath?.Invoke(false);
        Ctx.OnRespawn?.Invoke();
    }

    public override void CheckSwitchState() 
    {
        if (!Ctx.IsDead)
        {
            SwitchState(Factory.Idle());
        }
    }

    public override void InitializeSubState() 
    {

    }

    public IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(Ctx.RespawnDelay);
        Ctx.IsDead = false;
    }

}
