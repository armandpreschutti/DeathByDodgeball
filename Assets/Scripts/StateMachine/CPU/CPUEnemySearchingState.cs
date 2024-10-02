using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUEnemySearchingState : CPUBaseState
{
    float randomDistance;
    float stateTime;
    float exitTime;
    int newDirection;
    public CPUEnemySearchingState(CPUBrain currentContext, CPUStateFactory cpuStateFactory) : base(currentContext, cpuStateFactory)
    {

    }

    public override void EnterState()
    {
        randomDistance = Random.Range(3f, 7.2f);
        exitTime = Random.Range(0f,2f);
        Ctx.catchChance = Random.Range(1, 10);
        Ctx.dodgeChance = Random.Range(1, 10);
        Ctx.moveY = 1;
        newDirection = Ctx.courtCenter.position.y >= Ctx.transform.position.y ? 1 : -1;
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.CurrentSubState = "Enemy Searching State";
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
        if (!Ctx.playerStateMachine.IsDead )
        {
            stateTime += Time.deltaTime;
            if (stateTime >= exitTime)
            {
                SwitchState(Factory.Attacking());
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
            else if(!Ctx.playerStateMachine.IsEquipped)
            {
                SwitchState(Factory.Idle());
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
        if(Ctx.closestCatchableBall == null && Ctx.closestDodgableBall == null)
        {
            Ctx.moveY = newDirection;
        }
        
    }

}
