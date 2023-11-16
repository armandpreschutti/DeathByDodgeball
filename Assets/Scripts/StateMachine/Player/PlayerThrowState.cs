using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowState : PlayerBaseState
{
    public PlayerThrowState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {

    }
    public override void EnterState()
    {
        Ctx.Anim.SetBool("IsThrowing", true);
        Ctx.IsThrowing = true;
        Ctx.StartCoroutine(Throw());
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        SetPlayerDirection();
    }

    public override void FixedUpdateState()
    {
/*        Ctx.Rb.velocity = new Vector2(Ctx.AimDirection.x * Ctx.CurrentThrowPower /7f, 0f);*/
        Ctx.Rb.velocity = new Vector2(Ctx.MoveDirection.x * 5, Ctx.MoveDirection.y * 5);
    }

    public override void ExitState()
    {
       
    }

    public override void CheckSwitchState()
    {
        if (!Ctx.IsThrowing)
        {
            SwitchState(Factory.Idle());
        }
        if(Ctx.IsHurt) 
        {
            SwitchState(Factory.Hurt());
        }
    }

    public override void InitializeSubState()
    {

    }

    public IEnumerator Throw()
    {
   /*     float startTime = Time.time;

        while (Time.time < startTime + .25f)
        {
            if (Ctx.IsHurt)
            {
                Ctx.IsThrowing = false;
                break;
            }

            yield return null;
        }*/
        

        if (!Ctx.IsInvicible)
        {
            Ctx.CurrentStamina -= Ctx.CurrentThrowPower;
        }
        Ctx.OnThrow?.Invoke();
        Ctx.EquippedBall.GetComponent<BallStateMachine>().EnterActiveState(Ctx.CurrentThrowPower);
        Ctx.EquippedBall.GetComponent<BallStateMachine>().Rb.AddForce(Ctx.AimDirection * Ctx.CurrentThrowPower, ForceMode2D.Impulse);
        Ctx.UnequipBall(Ctx.EquippedBall);
        yield return new WaitForSeconds(Ctx.ThrowDuration);
        Ctx.HoldPosition = null;
        Ctx.CurrentThrowPower = Ctx.MinThrowPower;
        Ctx.Anim.SetBool("IsThrowing", false);
        Ctx.IsThrowing = false;

    }
    public void SetPlayerDirection()
    {
        bool flipped = Ctx.AimDirection.x < 0;
        Ctx.GetComponent<SpriteRenderer>().flipX = flipped;
    }
}
