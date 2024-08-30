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
        Ctx.Anim.SetBool("IsAiming", true);
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.CurrentSubState = "Aim State";

        SetAimDirection();
        SetThrowPower();
        Ctx.transform.Translate(Ctx.MoveDirection * (Ctx.CurrentThrowPower >= Ctx.MaxThrowPower ? Ctx.AimSpeed : Ctx.MoveSpeed) * Time.deltaTime);
        Ctx.SetPlayerOrientation();
    }

    public override void FixedUpdateState()
    {
      
    }

    public override void ExitState() 
    {
        //Ctx.HoldPosition = null;
        Ctx.IsAiming = false;
        Ctx.Anim.SetBool("IsAiming", false);

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
        if (Ctx.AimInput.magnitude > .75f)
        {
            flipped = Ctx.AimInput.x < 0 ? true : false;
        }
        else
        {
            flipped = Ctx.transform.position.x > 0f ? true : false;
        }
        Ctx.GetComponent<SpriteRenderer>().flipX = flipped;
        Ctx.HoldPosition = flipped ? Ctx.HoldRightPosition : Ctx.HoldLeftPosition;
        Ctx.AimDirection = new Vector3(flipped ? -1 : 1, 0, 0);

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
