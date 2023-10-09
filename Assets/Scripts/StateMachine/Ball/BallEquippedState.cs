using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEquippedState : BallBaseState
{
    public override void EnterState(BallStateMachine _ctx)
    {
        _ctx.Rb.velocity = Vector3.zero;
        _ctx.Col.enabled = false;
        _ctx.Rb.simulated = false;
    }

    public override void ExitState(BallStateMachine _ctx)
    {

    }

    public override void UpdateState(BallStateMachine _ctx)
    {

    }

    public override void OnTriggerEnter2D(BallStateMachine _ctx, Collider2D collider)
    {

    }
}
