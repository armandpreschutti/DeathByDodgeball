using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUIdleState : CPUBaseState
{

    public CPUIdleState(CPUBrain currentContext, CPUStateFactory cpuStateFactory) : base(currentContext, cpuStateFactory)
    {

    }


    public override void EnterState()
    {
        Debug.Log("CPU entered Idle State");
        Ctx.moveX = 0;
        Ctx.moveY = 0;
        Ctx.catchChance = Random.Range(1, 10);
        Ctx.dodgeChance = Random.Range(1, 10);
        Ctx.agressionChance = Random.Range(1, 10);
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.CurrentSubState = "Idle State";
    }

    public override void FixedUpdateState()
    {

    }


    public override void ExitState()
    {

    }

    public override void CheckSwitchState()
    {

        if (!Ctx.playerStateMachine.IsDead)
        {
            if (!Ctx.playerStateMachine.IsEquipped)
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
                else if (Ctx.closestCatchableBall != null && Ctx.catchChance <= Ctx.CatchSkill)
                {
                    if (!Ctx.playerStateMachine.IsCatching
                        && !Ctx.playerStateMachine.IsThrowing
                        && Ctx.playerStateMachine.CanCatch)
                    {
                        SwitchState(Factory.Catching());
                    }

                }
                else if (Ctx.agressionChance <= Ctx.AgressionSkill)
                {
                    SwitchState(Factory.BallSearching());
                }
                else
                {
                    SwitchState(Factory.Patroling());
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

}
