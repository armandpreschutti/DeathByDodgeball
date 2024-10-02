using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUPatrolState : CPUBaseState
{
    float randomDistance;
    float stateTime;
    float exitTime;
    int newDirection;

    public CPUPatrolState(CPUBrain currentContext, CPUStateFactory cpuStateFactory) : base(currentContext, cpuStateFactory)
    {

    }

    public override void EnterState()
    {

        randomDistance = Random.Range(3f, 7.2f);
        exitTime = Random.Range(1f, 1.5f);
        Ctx.catchChance = Random.Range(1, 10);
        Ctx.dodgeChance = Random.Range(1, 10);
        newDirection = Ctx.courtCenter.position.y >= Ctx.transform.position.y ? 1 : -1;
        Ctx.agressionChance = Random.Range(1, 10);

    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.CurrentSubState = "Patrolling State";

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
            if (!Ctx.playerStateMachine.IsEquipped)
            {
                stateTime += Time.deltaTime;
                if (Ctx.closestDodgableBall != null && Ctx.dodgeChance <= Ctx.DodgeSkill)
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
                if(stateTime >= exitTime)
                {
                    if (Ctx.agressionChance <= Ctx.AgressionSkill)
                    {
                        SwitchState(Factory.BallSearching());
                    }
                    else
                    {
                        SwitchState(Factory.Patroling());
                    }
                }
            }
            else if (Ctx.playerStateMachine.IsEquipped)
            {
                SwitchState(Factory.EnemySearching());
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
        if (Ctx.closestCatchableBall == null && Ctx.closestDodgableBall == null)
        {
            if (Ctx.isFacingLeft)
            {
                if (Ctx.transform.position.x >= randomDistance + .15f)
                {
                    Ctx.moveX = -1;
                }
                else if (Ctx.transform.position.x <= randomDistance - .15f)
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
                if (Ctx.transform.position.x <= -randomDistance - .15f)
                {
                    Ctx.moveX = 1;
                }
                else if (Ctx.transform.position.x >= -randomDistance + .15f)
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

    public void SetVeritcalMovement()
    {
        if (Ctx.closestCatchableBall == null && Ctx.closestDodgableBall == null)
        {
            Ctx.moveY = newDirection;
        }
    }
}
