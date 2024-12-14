using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUAttackingState : CPUBaseState
{
    float randomDistance;
    float randomPower;
    bool isAttackLevel;
    int randomState;

    public CPUAttackingState(CPUBrain currentContext, CPUStateFactory cpuStateFactory) : base(currentContext, cpuStateFactory)
    {

    }

    public override void EnterState()
    {
        randomPower = Random.Range(Ctx.playerStateMachine.MinThrowPower, Ctx.playerStateMachine.MaxThrowPower +5);
        randomState = Random.Range(1, 2);
        Ctx.playerStateMachine.IsThrowPressed = true;
        randomDistance = Random.Range(3f, 7.2f);
        Ctx.dodgeChance = Random.Range(1, 10);
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.CurrentSubState = "Attacking State";
        SetHorizontalMovement();
        SetVeritcalMovement();
        SetThrowPower();
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
        if(!Ctx.playerStateMachine.IsDead)
        {
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
            else if (!Ctx.playerStateMachine.IsAiming && !Ctx.playerStateMachine.IsThrowing)
            {
                Ctx.playerStateMachine.IsThrowPressed = false;
                SwitchState(Factory.Idle());
            }

        }
        else
        {
            Ctx.playerStateMachine.IsThrowPressed = false;
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
        if(Ctx.closestCatchableBall == null&& Ctx.closestDodgableBall == null)
        {
            if (Ctx.closestEnemy != null)
            {
                if (Ctx.closestEnemy.transform.position.y >= Ctx.transform.position.y )
                {
                    Ctx.moveY = 1;
                    isAttackLevel = false;

                }
                else if (Ctx.closestEnemy.transform.position.y <= Ctx.transform.position.y -.3)
                {
                    Ctx.moveY = -1;
                    isAttackLevel = false;
                }
                else
                {
                    Ctx.moveY = 0;
                    isAttackLevel = true;
                }
            }
            else
            {
                Ctx.moveY = 0;
            }

        }

    }

    public void SetThrowPower()
    {
        if (Ctx.playerStateMachine.CurrentThrowPower >= randomPower || isAttackLevel)
        {
            Ctx.playerStateMachine.IsThrowPressed = false;
        }
    }

}
