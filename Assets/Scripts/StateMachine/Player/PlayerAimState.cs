using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerAimState : PlayerBaseState
{
    public PlayerAimState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory): base(currentContext, playerStateFactory) 
    {

    }

    public override void EnterState()
    { 
        Ctx.CurrentThrowPower = Ctx.MinThrowPower;
        Ctx.IsAiming= true;
        Ctx.BaseAnim.SetBool("IsAiming", true);
        Ctx.SkinAnim.SetBool("IsAiming", true);
        Ctx.OnAim?.Invoke(true);    

    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.CurrentSubState = "Aim State";

        SetAimDirection();
        SetThrowPower();
    }

    public override void FixedUpdateState()
    {
        Ctx.Rb.velocity = Ctx.MoveDirection * (Ctx.CurrentThrowPower >= Ctx.MaxThrowPower ? Ctx.AimSpeed : Ctx.MoveSpeed);
    }

    public override void ExitState() 
    {
        Ctx.IsAiming = false;
        Ctx.BaseAnim.SetBool("IsAiming", false);
        Ctx.SkinAnim.SetBool("IsAiming", false);
        Ctx.OnAim?.Invoke(false);
/*        Ctx.AimRightPosition.gameObject.SetActive(false);
        Ctx.AimLeftPosition.gameObject.SetActive(false);*/
        Ctx.OnSuperState?.Invoke(false);
    }

    public override void CheckSwitchState()
    {
        if (!Ctx.IsThrowPressed)
        {
            SwitchState(Factory.Throw());
        }
        else if (Ctx.IsDead)
        {
            Ctx.IsThrowPressed = false;
            SwitchState(Factory.Death());
        }
        else if(Ctx.IsDodgePressed && !Ctx.IsExhausted) 
        {
            Debug.LogWarning("Pre Dodge exit logic called");
            Ctx.IsThrowPressed = false;
            Ctx.DodgeDirection = Ctx.MoveDirection;
            Ctx.IsSuper = false;
            Ctx.CurrentTarget = null;
            SwitchState(Factory.Dodge());
            Debug.LogWarning("Post Dodge exit logic called");
        }
    }

    public override void InitializeSubState() 
    {

    }    

    public void SetAimDirection()
    {
        bool flipped;
        flipped = Ctx.transform.position.x > 0f ? true : false;
        if (flipped)
        {
            Ctx.AimDirection = new Vector3(-1, 0, 0);
        }
        else
        {
            Ctx.AimDirection = new Vector3(1, 0, 0);
        }

    }

    public void SetThrowPower()
    {
        if (Ctx.IsAiming)
        {
            if (Ctx.CurrentThrowPower > Ctx.MaxThrowPower)
            {
                Ctx.CurrentThrowPower = Ctx.SuperThrowPower;

                if (!Ctx.IsSuper)
                {

                    Ctx.OnSuperState?.Invoke(true);
                    Ctx.IsSuper = true;
                }
            }
            else if (Ctx.CurrentThrowPower < Ctx.MinThrowPower)
            {
                Ctx.CurrentThrowPower = Ctx.MinThrowPower;
            }
            Ctx.CurrentThrowPower += Ctx.ThrowPowerIncreaseRate * Time.deltaTime;
        }

    }
    
}
