using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBallType : BallManager
{
    [Header("Freeze Ball Settings")]
    public float frozenSpeed;
    public float frozenTime;
    public ParticleSystem _normalTrail;
    public ParticleSystem _superTrail;
    public GameObject frozenInteraction;

    public override void PawnCollision(PlayerStateMachine stateMachine)
    {
        base.PawnCollision(stateMachine);
        Instantiate(frozenInteraction, transform.position, Quaternion.identity, null);
        if (stateMachine.TryGetComponent(out PawnAbilityHandler pawnAbilityHandler))
        {
            pawnAbilityHandler.SetFrozenState(isSuperBall, frozenSpeed, frozenTime);
        }
        owner.OnBallContact?.Invoke();
        Destroy(gameObject);
    }

    public override void EquiptBall(PlayerStateMachine stateMachine)
    {
        base.EquiptBall(stateMachine);
        SetBallTrailVFX(false);
    }

    public override void Launch(bool value, Vector2 direction, bool super, float power)
    {
        base.Launch(value, direction, super, power);
        _rb.AddForce(currentDirection * currentPower, ForceMode2D.Impulse);
        _rb.AddTorque(25f);
        SetBallTrailVFX(true);
    }

    public override void SelfDestruct()
    {
        Instantiate(frozenInteraction, transform.position, Quaternion.identity, null);
        if (owner.TryGetComponent(out PawnAbilityHandler pawnAbilityHandler))
        {
            pawnAbilityHandler.SetFrozenState(true, frozenSpeed, frozenTime);
        }
        owner.OnBallContact?.Invoke();
        Destroy(gameObject);
    }

    public void SetBallTrailVFX(bool value)
    {
        ParticleSystem particleSystem = isSuperBall ? _superTrail : _normalTrail;
        if (value)
        {
            particleSystem.Play();

        }
        else
        {
            particleSystem.Stop();
        }
    }

}
