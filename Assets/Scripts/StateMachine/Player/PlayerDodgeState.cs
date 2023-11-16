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
        Ctx.IsDodgePressed = false;
        Ctx.StartCoroutine(Dodge());
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

    }

    public override void CheckSwitchState() 
    {
        if(Ctx.IsDodging == false)
        {
            SwitchState(Factory.Idle());
        }
        if (Ctx.IsHurt)
        {
            Ctx.IsDodging = false;
            SwitchState(Factory.Hurt());
        }
    }

    public override void InitializeSubState() 
    {

    }

    IEnumerator Dodge()
    {
        Ctx.IsDodging = true;
        Ctx.Anim.SetBool("IsDodging", true);

        if (!Ctx.IsInvicible)
        {
            Ctx.CurrentStamina -= 20;
        }

        Ctx.OnDodge?.Invoke();
        Vector2 direction = Ctx.MoveDirection.normalized * Ctx.DodgeSpeed;
        float startTime = Time.time;

        while (Time.time < startTime + Ctx.DodgeDuration)
        {
            if (Ctx.IsHurt)
            {
                break;
            }

            Ctx.Rb.velocity = direction;
            yield return null;
        }

        Ctx.Anim.SetBool("IsDodging", false);
        Ctx.IsDodging = false;
    }
}
