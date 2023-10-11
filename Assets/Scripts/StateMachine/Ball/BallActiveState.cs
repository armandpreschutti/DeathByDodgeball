using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BallActiveState : BallBaseState
{

    public override void EnterState(BallStateMachine _ctx)
    {
        _ctx.transform.parent.GetComponent<Collider2D>();
        _ctx.Col.enabled = true;
        _ctx.Rb.simulated = true;
    }

    public override void ExitState(BallStateMachine _ctx)
    {

    }

    public override void UpdateState(BallStateMachine _ctx)
    {

    }

    public override void OnTriggerEnter2D(BallStateMachine _ctx, Collider2D collider)
    {
        if (collider.CompareTag("Player") && _ctx.Parent != collider.transform)
        {
            if (collider.GetComponent<PlayerStateMachine>().IsCatching)
            {
                collider.GetComponent<PlayerStateMachine>().EquipBall(_ctx.gameObject);
                _ctx.SwitchState(_ctx.EquippedState);                
            }
            else
            {
                collider.transform.GetComponent<PlayerManager>().GetComponent<HealthHandler>().TakeDamage(_ctx.BallDamage);
                GameObject.Destroy(_ctx.gameObject);
            }                
        }
        else if (collider.CompareTag("GarbageCollector"))
        {
            GameObject.Destroy(_ctx.gameObject);
        }
        else
        {
            return;
        }
    }
}

