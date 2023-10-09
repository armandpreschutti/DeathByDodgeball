using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallSelfDestructState : BallBaseState
{
    public override void EnterState(BallStateMachine _ctx)
    {
        if (_ctx.transform.parent.GetComponent<PlayerStateMachine>() != null)
        {
            _ctx.transform.parent.GetComponent<PlayerStateMachine>().TakeDamage(_ctx.BallDamage);
            _ctx.transform.parent.GetComponent<PlayerStateMachine>().UnequipBall(_ctx.gameObject);
            GameObject.Destroy(_ctx.gameObject);
        }
       
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

