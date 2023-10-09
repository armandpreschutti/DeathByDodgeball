using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAimState : PlayerBaseState
{
    public PlayerAimState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory): base(currentContext, playerStateFactory) 
    {

    }

    public override void EnterState()
    {
        Ctx.PlayerBar.gameObject.SetActive(true);
        Ctx.CurrentThrowPower = Ctx.MinThrowPower;
        Ctx.IsAiming= true;
        Ctx.Anim.SetBool("IsAiming", true);
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.Rb.velocity = new Vector2(Ctx.MoveDirection.x * Ctx.AimSpeed, Ctx.MoveDirection.y * Ctx.AimSpeed);
        Ctx.AimDirection= (Ctx.Target.position -  Ctx.transform.position).normalized;
        if (Ctx.IsAiming)
        {
            if (Ctx.CurrentThrowPower >= Ctx.MaxThrowPower)
            {
                Ctx.CurrentThrowPower = Ctx.MaxThrowPower;
                Ctx.IsAimPressed = false;
            }
            else
            {
                Ctx.CurrentThrowPower += Ctx.ThrowPowerIncreaseRate * Time.deltaTime;
                Ctx.PlayerBar.value = Ctx.CurrentThrowPower;
                if(Ctx.PlayerBar.value > 18f)
                {
                    Ctx.PlayerBar.fillRect.GetComponent<Image>().color = Color.red;
                }
            }
        }
        else
        {
            return;
        }        
    }

    public override void ExitState() 
    {
        Ctx.PlayerBar.fillRect.GetComponent<Image>().color = Color.white;
        Ctx.PlayerBar.gameObject.SetActive(false);
        Ctx.IsAiming = false;
        Ctx.Anim.SetBool("IsAiming", false);
        Ctx.CurrentThrowPower = Ctx.MinThrowPower;
    }

    public override void CheckSwitchState()
    {
        if (Ctx.IsAimPressed == false)
        {
            Throw();
            SwitchState(Factory.Idle());
        }
        else if(Ctx.IsHurt)
        {
            Ctx.IsAimPressed = false;
            SwitchState(Factory.Hurt());
        }
    }

    public override void InitializeSubState() 
    {

    }

    public void Throw()
    { 
        Ctx.EquippedBall.GetComponent<BallStateMachine>().EnterActiveState(Ctx.CurrentThrowPower);
        Ctx.EquippedBall.GetComponent<BallStateMachine>().Rb.AddForce(Ctx.AimDirection * Ctx.CurrentThrowPower, ForceMode2D.Impulse);
        Ctx.UnequipBall(Ctx.EquippedBall);
    }
}
