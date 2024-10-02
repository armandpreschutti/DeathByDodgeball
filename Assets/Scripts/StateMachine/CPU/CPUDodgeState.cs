using UnityEngine;

public class CPUDodgeState : CPUBaseState
{
    float stateTime;
    public CPUDodgeState(CPUBrain currentContext, CPUStateFactory cpuStateFactory) : base(currentContext, cpuStateFactory)
    {

    }

    public override void EnterState()
    {
        //Debug.Log("CPU entered Idle State");

        Ctx.moveX = 0;
        Ctx.moveY = Ctx.closestDodgableBall.transform.position.y > Ctx.transform.position.y ? 1 : -1;
        Ctx.playerStateMachine.IsDodgePressed = true;
    }

    public override void UpdateState()
    {
        CheckSwitchState();
        Ctx.CurrentSubState = "Dodging State";
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
            stateTime += Time.deltaTime;
            if (!Ctx.playerStateMachine.IsDodging && stateTime > .25f)
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
}
