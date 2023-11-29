using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class BallUnequippedState : BallBaseState
{
    public override void EnterState(BallStateMachine _ctx)
    {
        _ctx.SetSprite(0f);
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
        if (collider.CompareTag("Player"))
        {
            if (!collider.GetComponent<PlayerStateMachine>().IsEquipped)
            {
                _ctx.SwitchState(_ctx.EquippedState);
                collider.GetComponent<PlayerStateMachine>().EquipBall(_ctx.gameObject);
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }
}
