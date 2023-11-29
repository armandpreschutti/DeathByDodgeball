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
        Ctx.StartCoroutine(StopVelocityCoroutine());
        Ctx.Anim.SetBool("Die", true);
        //Ctx.Rb.velocity = Vector2.zero;
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
        Ctx.Anim.SetBool("Die", false);
        Ctx.OnRespawn?.Invoke();
    }

    public override void CheckSwitchState() 
    {
        if (!Ctx.IsDead)
        {
            SwitchState(Factory.Unequipped());
        }
    }

    public override void InitializeSubState() 
    {

    }
    public IEnumerator StopVelocityCoroutine()
    {
        yield return new WaitForSeconds(.5f);
        Ctx.Rb.velocity = Vector2.zero;
    }
}
