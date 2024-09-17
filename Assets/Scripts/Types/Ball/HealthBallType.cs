using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBallType : BallManager
{

    public override void EquiptBall(PlayerStateMachine stateMachine)
    {
        base.EquiptBall(stateMachine);
    }

    public override void Launch(bool value, Vector2 direction, bool super, float power)
    {
        base.Launch(value, direction, super, power);
        if(owner.TryGetComponent(out HealthSystem healthSystem))
        {
            healthSystem.AddLife();
            owner.OnHeal?.Invoke();
        }
        Destroy(gameObject);
    }

    public override void SelfDestruct()
    {
        base.SelfDestruct();
        if (owner.TryGetComponent(out HealthSystem healthSystem))
        {
            healthSystem.AddLife();
            owner.OnHeal?.Invoke();
        }
        Destroy(gameObject);
    }
}
