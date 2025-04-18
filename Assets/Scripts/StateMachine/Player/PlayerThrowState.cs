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
        //moveDirection = Ctx.MoveInput;
        moveDirection = Vector2.zero;

    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.CurrentSubState = "Throw State";
    }

    public override void FixedUpdateState()
    {
        //Ctx.Rb.velocity = Ctx.MoveDirection * Ctx.MoveSpeed;
        Ctx.Rb.velocity = Vector2.zero;

        
        //SetThrowDirection();
    }

    public override void ExitState()
    {

        Ctx.ThrowBall();
        Ctx.BaseAnim.SetBool("IsThrowing", false);
        Ctx.SkinAnim.SetBool("IsThrowing", false);
        Ctx.IsThrowing = false;
        Ctx.CurrentThrowPower = Ctx.MinThrowPower;
        Ctx.OnThrow?.Invoke(false);
        Ctx.IsSuper = false;
    }

    public override void CheckSwitchState()
    {
        if (Ctx.IsDead)
        {
            SwitchState(Factory.Death());
        }
        else
        {
            if (!Ctx.IsThrowing)
            {
                SwitchState(Factory.Idle());
            }
        }
    }

    public override void InitializeSubState()
    {

    }
    
  /*  public void SetThrowDirection()
    {
        bool flipped;
        flipped = Ctx.transform.position.x > 0f ? true : false;
        Vector3 throwDirection = new Vector3(flipped ? -1 : 1, 0, 0);
        Ctx.Rb.velocity = throwDirection * (Ctx.MoveSpeed * 1.5f);
    }*/
}
