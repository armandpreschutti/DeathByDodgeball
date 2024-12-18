using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBallType : BallManager
{
    [Header("Health Ball Settings")]
    public GameObject healInteraction;
    public ParticleSystem healSymbols;
    public GameObject aoeIndicator;
    public float aoeTriggerRadius;
    public float aoeNormalSize;
    public float aoeSuperSize;

    public override void Launch(bool value, Vector2 direction, bool super, float power)
    {
        base.Launch(value, direction, super, power);
        if(owner.TryGetComponent(out HealthSystem healthSystem))
        {
            if(isSuperBall)
            {
                HealNearbyPlayers();
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
            HealNearbyPlayers();
            owner.OnHeal?.Invoke();
        }
        InstantiateVFX();

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
                aoeIndicator.transform.localScale = new Vector3(aoeSuperSize, aoeSuperSize, aoeSuperSize);
            }
            else
            {
                aoeIndicator.GetComponent<SpriteRenderer>().color = normalAimColor;
                aoeIndicator.transform.localScale = new Vector3(aoeNormalSize, aoeNormalSize, aoeNormalSize);
            }
        }
        else
        {
            aoeIndicator.SetActive(false);
        }
    }

    void HealNearbyPlayers()
    {
        // Perform the OverlapSphere to detect all nearby colliders
        Collider2D[] colliders = Physics2D.OverlapCircleAll(aoeIndicator.transform.position, aoeTriggerRadius);

        // Create an array to hold players with the PawnManager component
        PawnManager[] playersToHeal = new PawnManager[colliders.Length];
        int count = 0;

        foreach (Collider2D collider in colliders)
        {
            // Check if the object has a PawnManager component
            PawnManager pawn = collider.GetComponent<PawnManager>();
            if (pawn != null)
            {
                playersToHeal[count] = pawn;
                count++;
            }
        }

        // Call Heal() on each detected player
        for (int i = 0; i < count; i++)
        {
            if (playersToHeal[i].GetComponent<HealthSystem>() != null && playersToHeal[i].teamId == owningTeam)
            {

                playersToHeal[i].GetComponent<HealthSystem>().AddLife();
                Instantiate(healInteraction, playersToHeal[i].transform.position + new Vector3(0f,.5f,0f), Quaternion.identity, null);
            }

        }
    }

    public void InstantiateVFX()
    {
        ParticleSystem symbolsInstance = Instantiate(healSymbols, owner.transform.position, Quaternion.identity, null);
        symbolsInstance.transform.parent = owner.transform;
        var emission = symbolsInstance.emission;
        emission.rateOverTime = isSuperBall? 3f : 1f;
    }
}
