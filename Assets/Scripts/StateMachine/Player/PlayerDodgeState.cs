using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerDodgeState : PlayerBaseState
{

    public PlayerDodgeState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) 
    {

    }

    public override void EnterState()
    {
        Ctx.BroadcastDodge();
        Ctx.IsDodgePressed = false;
        Ctx.IsDodging = true;
        Ctx.BaseAnim.SetBool("IsDodging", true);
        Ctx.SkinAnim.SetBool("IsDodging", true);
        Ctx.OnDodge?.Invoke(true);
        //Ctx.CurrentTarget = null;
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.CurrentSubState = "Dodge State";
       

    }

    public override void FixedUpdateState()
    {
        Ctx.Rb.velocity = Ctx.DodgeDirection * Ctx.DodgeSpeed;
    }


    public override void ExitState() 
    {
        Ctx.IsDodging = false;
        Ctx.BaseAnim.SetBool("IsDodging", false);
        Ctx.SkinAnim.SetBool("IsDodging", false);
        Ctx.OnDodge?.Invoke(false);
    }

    public override void CheckSwitchState() 
    {
        if(Ctx.IsDead)
        {
            SwitchState(Factory.Death());
        }
        else
        {
            if (Ctx.IsDodging == false)
            {
                if (Ctx.MoveInput != Vector2.zero)
                {
                    SwitchState(Factory.Move());
                }
                else
                {
                    SwitchState(Factory.Idle());
                }
            }
            else if(Ctx.IsCatching == true)
            {
                SwitchState(Factory.Catch());
            }
        }       
    }

    public override void InitializeSubState() 
    {

    }

}
