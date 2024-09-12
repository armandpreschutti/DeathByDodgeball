using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowState : PlayerBaseState
{
    Vector2 moveDirection;
    public PlayerThrowState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {

    }
    public override void EnterState()
    {
        Ctx.BaseAnim.SetBool("IsThrowing", true);
        Ctx.SkinAnim.SetBool("IsThrowing", true);
        Ctx.IsThrowing = true;
        Ctx.OnThrow?.Invoke(true);
        moveDirection = Ctx.MoveInput;

    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.CurrentSubState = "Throw State";
    }

    public override void FixedUpdateState()
    {
        SetThrowDirection();
    }

    public override void ExitState()
    {

        Ctx.ThrowBall();
        Ctx.BaseAnim.SetBool("IsThrowing", false);
        Ctx.SkinAnim.SetBool("IsThrowing", false);
        Ctx.IsThrowing = false;
        Ctx.CurrentThrowPower = Ctx.MinThrowPower;
        Ctx.OnThrow?.Invoke(false);
        Ctx.CurrentTarget = null;
        Ctx.IsSuper = false;
    }

    public override void CheckSwitchState()
    {
        if (!Ctx.IsThrowing)
        {
            SwitchState(Factory.Idle());
        }

    }

    public override void InitializeSubState()
    {

    }
    
    public void SetThrowDirection()
    {
/*        if (Ctx.CurrentTarget != null)
        {
            
            Ctx.Rb.velocity = (Ctx.CurrentTarget.transform.position - Ctx.transform.position).normalized * (Ctx.MoveSpeed * 1.5f);
        }
        else
        {
            bool flipped;
            flipped = Ctx.transform.position.x > 0f ? true : false;
            Vector3 throwDirection = new Vector3(flipped ? -1 : 1, 0, 0);
            Ctx.Rb.velocity = throwDirection * (Ctx.MoveSpeed * 1.5f);
        }*/
        Ctx.Rb.velocity = moveDirection * (Ctx.MoveSpeed * 1.5f);


    }
}
