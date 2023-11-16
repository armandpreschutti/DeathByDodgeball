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
        if(_ctx.BallDamage >= 35)
        {
            _ctx.SuperBallVFX.Play();
        }
        else
        {
            _ctx.NormalBallVFX.Play();
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
        if (collider.CompareTag("Player") && _ctx.Parent != collider.transform)
        {
            if (collider.GetComponent<PlayerStateMachine>().IsCatching)
            {
                collider.GetComponent<PlayerStateMachine>().EquipBall(_ctx.gameObject);
                _ctx.SwitchState(_ctx.EquippedState);                
            }
            else
            {                
                if (!collider.GetComponent<HealthHandler>().IsInvicible)
                { 
                    bool hitDirection = _ctx.transform.position.x > 0;
                    collider.transform.GetComponent<PlayerManager>().PlayerStateMachine.IsHurt = true;
                    collider.transform.GetComponent<PlayerManager>().HealthHandler.KillPlayer(_ctx.BallDamage);
                    collider.GetComponent<PlayerManager>().PlayerStateMachine.Rb.AddForce(new Vector3(hitDirection ? 1 : -1, 0, 0) * (_ctx.BallDamage * _ctx.KnockBackPower), ForceMode2D.Force);
                    GameObject.Destroy(_ctx.gameObject);
                }
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

