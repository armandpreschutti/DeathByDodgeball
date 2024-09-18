using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class NormalBallType : BallManager
{
    [Header("Normal Ball Settings")]
    public GameObject _hitVfx;
    public GameObject _explosionVfx;
    public ParticleSystem _normalTrail;
    public ParticleSystem _superTrail;

    public override void PawnCollision(PlayerStateMachine stateMachine)
    {
        base.PawnCollision(stateMachine);
        stateMachine.IsDead = true;
        stateMachine.Rb.AddForce(new Vector2(stateMachine.transform.position.x - transform.position.x, 0).normalized * currentPower, ForceMode2D.Impulse);
        isBallActive = false;
        if (isSuperBall)
        {
            Instantiate(_explosionVfx, transform.position, Quaternion.identity, null);
            onExplosion?.Invoke();
            Destroy(gameObject);
        }
        else
        {
            Instantiate(_hitVfx, transform.position, Quaternion.identity, null);
            owner.OnBallContact?.Invoke();
            _rb.velocity = new Vector2(_rb.velocity.x, originPoint.y < transform.position.y ? 7.5f : -7.5f);
            Destroy(gameObject, 1f);
        }
    }

    public override void BallCollision()
    {
        base.BallCollision();
        if (isSuperBall)
        {
            Instantiate(_explosionVfx, transform.position, Quaternion.identity, null);
            onExplosion?.Invoke();
            Destroy(gameObject);

        }
        else
        {
            Instantiate(_hitVfx, transform.position, Quaternion.identity, null);
            _rb.velocity = new Vector2(-_rb.velocity.x, 0);
        }
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

        SetBallTrailVFX(true);
    }

    public override void SelfDestruct()
    {
        owner.IsDead = true;
        owner.Rb.AddForce(new Vector2(owner.transform.position.x - transform.position.x, 0).normalized * currentPower, ForceMode2D.Impulse);
        isBallActive = false;
        Instantiate(_explosionVfx, transform.position, Quaternion.identity, null);
        onExplosion?.Invoke();
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
