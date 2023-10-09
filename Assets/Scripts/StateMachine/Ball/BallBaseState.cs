using UnityEngine;

public abstract class BallBaseState
{
    public abstract void EnterState(BallStateMachine _ctx);

    public abstract void UpdateState(BallStateMachine _ctx);
    
    public abstract void ExitState(BallStateMachine _ctx);
    
    public abstract void OnTriggerEnter2D(BallStateMachine _ctx, Collider2D collider);
}
