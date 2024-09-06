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
        //Ctx.transform.Translate(Ctx.MoveDirection * (Ctx.CurrentThrowPower >= Ctx.MaxThrowPower ? Ctx.AimSpeed : Ctx.MoveSpeed) * Time.deltaTime);

        Ctx.SetPlayerOrientation();
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
        else if(Ctx.IsDodgePressed) 
        {
            Ctx.IsThrowPressed = false;
            SwitchState(Factory.Dodge());
        }
    }

    public override void InitializeSubState() 
    {

    }

    

    public void SetAimDirection()
    {
        bool flipped;
        flipped = Ctx.transform.position.x > 0f ? true : false;
/*        if (Ctx.AimInput.magnitude > .75f)
        {
            flipped = Ctx.AimInput.x < 0 ? true : false;
        }
        else
        {
            flipped = Ctx.transform.position.x > 0f ? true : false;
        }*/
        Ctx.GetComponent<SpriteRenderer>().flipX = flipped;
        Ctx.HoldPosition = flipped ? Ctx.HoldRightPosition : Ctx.HoldLeftPosition;
        if (flipped)
        {
            Ctx.AimRightPosition.gameObject.SetActive(false);
            Ctx.AimLeftPosition.gameObject.SetActive(true);
        }
        else
        {
            Ctx.AimRightPosition.gameObject.SetActive(true);
            Ctx.AimLeftPosition.gameObject.SetActive(false);
        }
        if(Ctx.CurrentTarget != null)
        {
            //Set aim direction to target position normalized
            Ctx.AimDirection = (Ctx.CurrentTarget.transform.position - Ctx.transform.position).normalized;
        }
        else
        {
            Ctx.AimDirection = new Vector3(flipped ? -1 : 1, 0, 0);
        }

    }
    public void SetThrowPower()
    {
        if (Ctx.CurrentThrowPower >= Ctx.MaxThrowPower)
        {
            Ctx.CurrentThrowPower = Ctx.SuperThrowPower;
        }
        Ctx.CurrentThrowPower += Ctx.ThrowPowerIncreaseRate * Time.deltaTime;
    }
    
}
