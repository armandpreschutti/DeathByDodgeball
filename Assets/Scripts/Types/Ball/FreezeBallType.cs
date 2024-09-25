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
    public GameObject AimLeftPosition;
    public GameObject AimRightPosition;

    public override void PawnCollision(PlayerStateMachine stateMachine)
    {
        base.PawnCollision(stateMachine);
        Instantiate(frozenInteraction, transform.position, Quaternion.identity, null);
        if (stateMachine.TryGetComponent(out PawnAbilityManager pawnAbilityHandler))
        {
            pawnAbilityHandler.SetFrozenState(isSuperBall, frozenSpeed, frozenTime);
        }
        if(isSuperBall)
        {
            onExplosion?.Invoke();
        }
        else
        {
            owner.OnBallContact?.Invoke();
            stateMachine.OnBallContact?.Invoke();
        }

        Destroy(gameObject);
    }

    public override void EquiptBall(PlayerStateMachine stateMachine)
    {
        base.EquiptBall(stateMachine);
        DisableTrailVFX();
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
        if (owner.TryGetComponent(out PawnAbilityManager pawnAbilityHandler))
        {
            pawnAbilityHandler.SetFrozenState(true, frozenSpeed, frozenTime);
        }
        owner.OnBallContact?.Invoke();
        Destroy(gameObject);
    }
    public override void SetAimIndicator()
    {
        base.SetAimIndicator();
        bool flipped;
        flipped = transform.position.x > 0f ? true : false;
        if (owner != null && owner.IsAiming)
        {
            if (flipped)
            {
                AimRightPosition.SetActive(false);
                AimLeftPosition.SetActive(true);
                if (owner.IsSuper)
                {
                    AimLeftPosition.GetComponent<SpriteRenderer>().color = superAimColor;
                }
                else
                {
                    AimLeftPosition.GetComponent<SpriteRenderer>().color = normalAimColor;
                }
            }
            else
            {
                AimRightPosition.SetActive(true);
                AimLeftPosition.SetActive(false);
                if (owner.IsSuper)
                {
                    AimRightPosition.GetComponent<SpriteRenderer>().color = superAimColor;
                }
                else
                {
                    AimRightPosition.GetComponent<SpriteRenderer>().color = normalAimColor;
                }
            }
        }
        else
        {
            AimRightPosition.SetActive(false);
            AimLeftPosition.SetActive(false);
        }
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

    public void DisableTrailVFX()
    {
        _superTrail.Stop();
        _normalTrail.Stop();
    }

}
