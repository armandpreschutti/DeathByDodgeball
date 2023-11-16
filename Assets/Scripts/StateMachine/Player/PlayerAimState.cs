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
        Ctx.PlayerThrowBar.gameObject.SetActive(true);
        Ctx.CurrentThrowPower = Ctx.MinThrowPower;
        Ctx.IsAiming= true;
        Ctx.Anim.SetBool("IsAiming", true);
        Ctx.AimingObject.SetActive(true);
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        SetAimDirection();
        SetThrowPower();
    }

    public override void FixedUpdateState()
    {
        if (Ctx.CurrentThrowPower >= Ctx.MaxThrowPower)
        {
            Ctx.Rb.velocity = new Vector2(Ctx.MoveDirection.x * Ctx.AimSpeed, Ctx.MoveDirection.y * Ctx.AimSpeed);
        }
        else
        {
            Ctx.Rb.velocity = new Vector2(Ctx.MoveDirection.x * Ctx.IdleSpeed, Ctx.MoveDirection.y * Ctx.IdleSpeed);
        }
    }

    public override void ExitState() 
    {
        Ctx.HoldPosition = null;
        Ctx.PlayerThrowBar.fillRect.GetComponent<Image>().color = Color.white;
        Ctx.PlayerThrowBar.gameObject.SetActive(false);
        Ctx.IsAiming = false;
        Ctx.Anim.SetBool("IsAiming", false);
        Ctx.AimingObject.SetActive(false);

    }

    public override void CheckSwitchState()
    {
        if (!Ctx.IsAimPressed)
        {
            SwitchState(Factory.Throw());
        }
        else if(Ctx.IsHurt)
        {
            Ctx.IsAimPressed = false;
            SwitchState(Factory.Hurt());
        }
        else if(Ctx.IsDodgePressed) 
        {
            Ctx.IsAimPressed = false;
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
            Debug.Log("manual aiming");
            if (Ctx.AimInput.x < 0)
            {
                flipped = true;
            }
            else
            {
                flipped = false;
            }
        }
        else
        {
            Debug.Log("Auto aiming");
            if (Ctx.transform.position.x > 0f)
            {
                flipped = true;
            }
            else
            {
                flipped = false;
            }
        }
        Ctx.GetComponent<SpriteRenderer>().flipX = flipped;
        Ctx.HoldPosition = flipped ? Ctx.HoldRightPosition : Ctx.HoldLeftPosition;
        Ctx.AimDirection = new Vector3(flipped ? -1 : 1, 0, 0);
        Ctx.AimingObject.transform.localPosition = Ctx.AimDirection + new Vector3(flipped ? - 10f : 10f, .25f, 0);
        Ctx.PlayerThrowBar.transform.localPosition = new Vector3(flipped ? 400 : -400, 0, 0);

        if (Ctx.IsEquipped)
        {
            Ctx.ChangeBallPosition(Ctx.EquippedBall);
        }
       
    }
    public void SetThrowPower()
    {
        if (!Ctx.IsThrowing)
        {

            if (Ctx.CurrentThrowPower >= Ctx.MaxThrowPower)
            {
                Ctx.CurrentThrowPower = Ctx.SuperThrowPower;
                Ctx.PlayerThrowBar.fillRect.GetComponent<Image>().color = Color.red;
            }

            if (Ctx.CurrentThrowPower >= Ctx.CurrentStamina)
            {
                Ctx.CurrentStamina = 0;
            }

            Ctx.CurrentThrowPower += Ctx.ThrowPowerIncreaseRate * Time.deltaTime;
            Ctx.PlayerThrowBar.value = Ctx.CurrentThrowPower;

        }
        else
        {
            return;
        }
    }
    
}
