using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBallType : BallManager
{

    [Header("Energy Ball Settings")]
    public float energizedSpeed;
    public float energizedDodgeSpeed;
    public float energizedThrowRate;
    public float energizedTime;
    public GameObject energizedInteraction; 

    public override void Launch(bool value, Vector2 direction, bool super, float power)
    {
        base.Launch(value, direction, super, power);
        if (owner.TryGetComponent(out PawnAbilityHandler pawnAbilityHandler))
        {
            pawnAbilityHandler.SetEnergizedState(isSuperBall, energizedSpeed, energizedDodgeSpeed, energizedThrowRate, energizedTime);
        }
        Instantiate(energizedInteraction, owner.transform.position, Quaternion.identity, null);
        Destroy(gameObject);
    }

 
    public override void SelfDestruct()
    {
        base.SelfDestruct();
        if (owner.TryGetComponent(out PawnAbilityHandler pawnAbilityHandler))
        {
            pawnAbilityHandler.SetEnergizedState(isSuperBall, energizedSpeed, energizedDodgeSpeed, energizedThrowRate, energizedTime);
        }
        Destroy(gameObject);
    }

}
