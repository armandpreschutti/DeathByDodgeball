using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUBallSearchingState : CPUBaseState
{
    Coroutine testRoutine;
    int catchChance;

    public CPUBallSearchingState(CPUBrain currentContext, CPUStateFactory cpuStateFactory) : base(currentContext, cpuStateFactory)
    {

    }

    public override void EnterState()
    {
        catchChance = Random.Range(1, 10);
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.CurrentSubState = "Ball Searching State";
        SetVeritcalMovement();
        SetHorizontalMovement();
    }

    public override void FixedUpdateState()
    {

    }


    public override void ExitState()
    {
        Ctx.moveY = 0;
        Ctx.moveX = 0;
    }

    public override void CheckSwitchState()
    {
        if (!Ctx.playerStateMachine.IsDead)
        {
            if (Ctx.playerStateMachine.IsEquipped)
            {
                SwitchState(Factory.EnemySearching());
            }
            else if (Ctx.closestDodgableBall != null && Ctx.dodgeChance <= Ctx.DodgeSkill)
            {
                if (!Ctx.playerStateMachine.IsDodging
                    && !Ctx.playerStateMachine.IsCatching
                    && !Ctx.playerStateMachine.IsThrowing)
                {
                    Ctx.moveY = 0;
                    Ctx.moveX = 0;
                    SwitchState(Factory.Dodging());
                }
            }
            else if (Ctx.closestCatchableBall != null && Ctx.catchChance <= Ctx.CatchSkill)
            {
                if (!Ctx.playerStateMachine.IsCatching
                    && !Ctx.playerStateMachine.IsThrowing
                    && Ctx.playerStateMachine.CanCatch)
                {
                    SwitchState(Factory.Catching());
                }

            }
        }
        else
        {
            SwitchState(Factory.Death());
        }


    }

    public override void InitializeSubState()
    {

    }
    public void SetHorizontalMovement()
    {
        if(Ctx.closestCatchableBall == null && Ctx.closestDodgableBall == null)
        {
            if (Ctx.closestFreeBall != null)
            {
                if (Ctx.isFacingLeft)
                {
                    if (Ctx.transform.position.x >= Ctx.courtCenter.position.x + .4)
                    {
                        Ctx.moveX = -1;
                    }
                    else
                    {
                        Ctx.moveX = 0;
                    }
                }
                else
                {
                    if (Ctx.transform.position.x <= Ctx.courtCenter.position.x - .4)
                    {
                        Ctx.moveX = 1;
                    }
                    else
                    {
                        Ctx.moveX = 0;
                    }
                }
            }
            else
            {
                if (Ctx.isFacingLeft)
                {
                    if (Ctx.transform.position.x <= 3f)
                    {
                        Ctx.moveX = 1;
                    }
                    else
                    {
                        Ctx.moveX = 0;
                    }
                }
                else
                {
                    if (Ctx.transform.position.x >= -3f)
                    {
                        Ctx.moveX = -1;
                    }
                    else
                    {
                        Ctx.moveX = 0;
                    }
                }
            }
        }

       
    }

    public void SetVeritcalMovement()
    {
        if (Ctx.closestCatchableBall == null && Ctx.closestDodgableBall == null)
        {
            if (Ctx.closestFreeBall != null)
            {
                if (Ctx.closestFreeBall.transform.position.y >= Ctx.transform.position.y + .3)
                {
                    Ctx.moveY = 1;
                }
                else if (Ctx.closestFreeBall.transform.position.y <= Ctx.transform.position.y + .1)
                {
                    Ctx.moveY = -1;
                }
                else
                {
                    Ctx.moveY = 0;
                }
            }
        }
    }
}
