using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerCatchState : PlayerBaseState
{
    public PlayerCatchState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory)
    {

    }

    public override void EnterState()
    {
        Ctx.IsCatchPressed = false;
        Ctx.IsCatching = true;
        Ctx.BaseAnim.SetBool("IsCatching", true);
        Ctx.SkinAnim.SetBool("IsCatching", true);
        Ctx.CatchArea.gameObject.SetActive(true);
        Ctx.OnCatch?.Invoke(true);
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.CurrentSubState = "Catch State";
    }

    public override void FixedUpdateState()
    {
        if (Ctx.ClosestBall != null)
        {
            Vector2 direction = (new Vector2(Ctx.ClosestBall.transform.position.x, Ctx.ClosestBall.transform.position.y) - Ctx.Rb.position).normalized;
            Ctx.Rb.velocity = direction * Ctx.DodgeSpeed;
        }
        else
        {
            Ctx.Rb.velocity = Ctx.MoveDirection * Ctx.MoveSpeed;
          //  Ctx.Rb.velocity = Vector2.zero;
        }
    }

    public override void ExitState()
    {
        Ctx.BaseAnim.SetBool("IsCatching", false);
        Ctx.SkinAnim.SetBool("IsCatching", false);
        Ctx.IsCatching = false;
        Ctx.CatchArea.gameObject.SetActive(false);
        Ctx.OnCatch?.Invoke(false);
    }

    public override void CheckSwitchState()
    {
        if (Ctx.IsDead)
        {
            SwitchState(Factory.Death());
        }
        else
        {
            if (Ctx.IsCatching == false)
            {
                SwitchState(Factory.Idle());
            }
            else if (Ctx.IsEquipped)
            {
                SwitchState(Factory.Idle());
            }
        }       
    }

    public override void InitializeSubState()
    {

    }
}