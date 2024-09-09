using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState: PlayerBaseState
{
    public PlayerDeathState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) 
    {
        IsRootState = true;
        InitializeSubState();
    }

    public override void EnterState()
    { 
        Ctx.DestroyBall();
        Ctx.BaseAnim.SetBool("Die", true);
        Ctx.SkinAnim.SetBool("Die", true);
        Ctx.OnDeath?.Invoke(true);
        Ctx.MoveInput = Vector2.zero;
        if(Ctx.CanRespawn)
        {
            Ctx.StartCoroutine(RespawnAfterDelay());
        }
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.CurrentSuperState = "Death State";
    }

    public override void FixedUpdateState()
    {
        Ctx.Rb.velocity = Vector2.zero;
    }


    public override void ExitState() 
    {
        Ctx.BaseAnim.SetBool("Die", false);
        Ctx.SkinAnim.SetBool("Die", false);
        Ctx.OnDeath?.Invoke(false);
        Ctx.OnRespawn?.Invoke();
    }

    public override void CheckSwitchState() 
    {
        if (!Ctx.IsDead)
        {
            SwitchState(Factory.Active());
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
