using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangType : BallManager
{
    [Header("Boomerang Ball Settings")]
    public GameObject _hitVfx;
    public GameObject _explosionVfx;
    public ParticleSystem _normalTrail;
    public ParticleSystem _superTrail;
    public GameObject AimLeftPosition;
    public GameObject AimRightPosition;
    public GameObject originalOwner;
    public bool isRebounding;


    public override void PawnCollision(PlayerStateMachine stateMachine)
    {
        base.PawnCollision(stateMachine);
        if (stateMachine.GetComponent<HealthSystem>() != null)
        {
            HealthSystem healthSystem = stateMachine.GetComponent<HealthSystem>();
            if (healthSystem.currentLives == 1)
            {
                onPlayerKill?.Invoke(owner.GetComponent<PawnManager>().slotId);
            }
        }
        stateMachine.IsDead = true;
        //stateMachine.Rb.AddForce(new Vector2(stateMachine.transform.position.x - transform.position.x, 0).normalized * currentPower, ForceMode2D.Impulse);

        if (isSuperBall)
        {
            Instantiate(_hitVfx, transform.position, Quaternion.identity, null);
            onExplosion?.Invoke();
            _rb.velocity = new Vector2(-_rb.velocity.x /4, 0);
            isRebounding = true;
            isBallCatchable = true;
        }
        else
        {
            Instantiate(_hitVfx, transform.position, Quaternion.identity, null);
            owner.OnBallContact?.Invoke();
            _rb.velocity = new Vector2(-_rb.velocity.x / 1.5f, 0);
            isBallCatchable= true;
        }

    }

    public override void BallCollision(BallManager ballManager)
    {
        base.BallCollision(ballManager);
        if (isSuperBall)
        {
            if (ballManager.isSuperBall)
            {
                Instantiate(_explosionVfx, transform.position, Quaternion.identity, null);
                onExplosion?.Invoke();
                Destroy(gameObject);
            }
        }
        else
        {
            Instantiate(_hitVfx, transform.position, Quaternion.identity, null);
            isBallCatchable = true;
            _rb.velocity = new Vector2(-_rb.velocity.x, 0);
        }
    }

    public override void EquiptBall(PlayerStateMachine stateMachine)
    {
        base.EquiptBall(stateMachine);
        DisableTrailVFX();
        originalOwner = owner.gameObject;
        isRebounding = false;
    }

    public override void Launch(bool value, Vector2 direction, bool super, float power)
    {
        base.Launch(value, direction, super, power);
        _rb.AddForce(currentDirection * currentPower, ForceMode2D.Impulse);

        SetBallTrailVFX(true);
    }

    public override void SelfDestruct()
    {
        owner.IsDead = true;
        isBallActive = false;
        Instantiate(_explosionVfx, transform.position, Quaternion.identity, null);
        onExplosion?.Invoke();
        Destroy(gameObject);
    }
    public override void SetAimIndicator()
    {
        base.SetAimIndicator();
        bool flipped;
        flipped = transform.position.x > 0f ? true : false;
        if (owner != null && owner.IsAiming && !isBallActive)
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

    public override void Update()
    {
        base.Update();
        if (isBallActive)
        {
            SetTrajectory();
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

    public void SetTrajectory()
    {

        if (originalOwner != null && isRebounding)
        {
            // Get the current position of the object
            Vector2 currentPosition = _rb.position;

            // Get the Y position of the target
            float targetY = originalOwner.transform.position.y + .5f;

            // Calculate the direction along Y axis only
            float directionY = targetY - currentPosition.y;

            // Apply velocity in the Y axis towards the target
            _rb.velocity = new Vector2(_rb.velocity.x, directionY * (currentPower / 3));
        }
    }
}
