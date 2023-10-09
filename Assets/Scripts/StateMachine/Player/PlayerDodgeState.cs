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
        //Debug.Log("Entered Dodge State");
        Ctx.IsDodgePressed = false;
        Ctx.StartCoroutine(Dodge());
        Ctx.StartCoroutine(RefillDodge());
    }

    public override void UpdateState()
    {
        CheckSwitchState();
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
    }

    public override void InitializeSubState() 
    {

    }

    IEnumerator Dodge()
    {
        Ctx.IsDodging = true;
        Ctx.Anim.SetBool("IsDodging", true);
        Ctx.TotalDodges -= 1;
        Vector2 direction = Ctx.MoveDirection.normalized * Ctx.DodgeSpeed;
        Ctx.DodgeSlider.value = Ctx.TotalDodges;
        float startTime = Time.time;

        while (Time.time < startTime + Ctx.DodgeDuration)
        {

            // Check if IsHurt is true, and if so, exit the loop
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

    IEnumerator RefillDodge()
    {
        yield return new WaitForSeconds(Ctx.DodgeRefillRate);
        Ctx.TotalDodges += 1;
        Ctx.DodgeSlider.value = Ctx.TotalDodges;
    }
}
