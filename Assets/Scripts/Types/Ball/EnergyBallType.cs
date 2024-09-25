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
    public GameObject aoeIndicator;

    public override void Launch(bool value, Vector2 direction, bool super, float power)
    {
        base.Launch(value, direction, super, power);
        if (owner.TryGetComponent(out PawnAbilityManager pawnAbilityHandler))
        {
            pawnAbilityHandler.SetEnergizedState(isSuperBall, energizedSpeed, energizedDodgeSpeed, energizedThrowRate, energizedTime);
        }
        Instantiate(energizedInteraction, owner.transform.position, Quaternion.identity, null);
        Destroy(gameObject);
    }

 
    public override void SelfDestruct()
    {
        base.SelfDestruct();
        if (owner.TryGetComponent(out PawnAbilityManager pawnAbilityHandler))
        {
            pawnAbilityHandler.SetEnergizedState(isSuperBall, energizedSpeed, energizedDodgeSpeed, energizedThrowRate, energizedTime);
        }
        Destroy(gameObject);
    }

    public override void SetAimIndicator()
    {
        base.SetAimIndicator();
        if (owner != null && owner.IsAiming)
        {
            aoeIndicator.SetActive(true);
            if (owner.IsSuper)
            {
                aoeIndicator.GetComponent<SpriteRenderer>().color = superAimColor;
            }
            else
            {
                aoeIndicator.GetComponent<SpriteRenderer>().color = normalAimColor;
            }
        }
        else
        {
            aoeIndicator.SetActive(false);
        }
    }
}
