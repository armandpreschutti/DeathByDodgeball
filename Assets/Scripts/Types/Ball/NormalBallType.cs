using UnityEngine;

public class NormalBallType : BallManager
{
    [Header("Normal Ball Settings")]
    public GameObject _hitVfx;
    public GameObject _explosionVfx;
    public ParticleSystem _normalTrail;
    public ParticleSystem _superTrail;
    public GameObject AimLeftPosition;
    public GameObject AimRightPosition;

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
            //_rb.velocity = new Vector2(-_rb.velocity.x, 0);
            _rb.velocity = new Vector2(_rb.velocity.x, ballManager.transform.position.y < transform.position.y ? 7.5f : -7.5f);
        }
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
    public override void SetAimIndicator()
    {
        base.SetAimIndicator();
        bool flipped;
        flipped = transform.position.x > 0f ? true : false;
        if (owner!= null && owner.IsAiming && !isBallActive)
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
