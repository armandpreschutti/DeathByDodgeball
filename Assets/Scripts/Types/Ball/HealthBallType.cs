using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBallType : BallManager
{
    [Header("Health Ball Settings")]
    public GameObject healInteraction;
    public ParticleSystem healSymbols;

    public override void Launch(bool value, Vector2 direction, bool super, float power)
    {
        base.Launch(value, direction, super, power);
        if(owner.TryGetComponent(out HealthSystem healthSystem))
        {
            if(isSuperBall)
            {
                healthSystem.RefillLives();
                Instantiate(healInteraction, owner.transform.position, Quaternion.identity, null);
            }
            else
            {
                healthSystem.AddLife();      
            }
        }
        owner.OnHeal?.Invoke();
        InstantiateVFX();

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

    public void InstantiateVFX()
    {
        ParticleSystem symbolsInstance = Instantiate(healSymbols, owner.transform.position, Quaternion.identity, null);
        symbolsInstance.transform.parent = owner.transform;
        var emission = symbolsInstance.emission;
        emission.rateOverTime = isSuperBall? 3f : 1f;
    }
}
