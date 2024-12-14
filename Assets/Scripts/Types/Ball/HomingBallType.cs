using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HomingBallType : BallManager
{
    [Header("Homing Ball Settings")]
    public GameObject _hitVfx;
    public GameObject _explosionVfx;
    public ParticleSystem _normalTrail;
    public ParticleSystem _superTrail;
    public GameObject currentTarget;
/*    public GameObject AimLeftPosition;
    public GameObject AimRightPosition;*/
    public GameObject CrossHair;

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
        isBallActive = false;

        if (isSuperBall)
        {
            Instantiate(_explosionVfx, transform.position, Quaternion.identity, null);
            onExplosion?.Invoke();
            //_rb.velocity = new Vector2(-_rb.velocity.x /3, 0);            
            isSuperBall = false;
            Destroy(gameObject);
        }
        else
        {
            Instantiate(_hitVfx, transform.position, Quaternion.identity, null);
            owner.OnBallContact?.Invoke();
            //_rb.velocity = new Vector2(_rb.velocity.x, originPoint.y < transform.position.y ? 7.5f : -7.5f);
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
        currentTarget = null;
        isBallCatchable = false;
        CrossHair.SetActive(false);
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
        isBallActive = false;
        Instantiate(_explosionVfx, transform.position, Quaternion.identity, null);
        onExplosion?.Invoke();
        CrossHair.transform.parent = transform;
        CrossHair.SetActive(false);
        Destroy(gameObject);
    }

    public override void SetAimIndicator()
    {
        base.SetAimIndicator();
        bool flipped;
        flipped = transform.position.x > 0f ? true : false;
        if (owner != null && owner.IsAiming)
        {

            if (currentTarget != null)
            {
                CrossHair.SetActive(true);
                CrossHair.transform.parent = currentTarget.transform;
                CrossHair.transform.localPosition = new Vector3(0, .4f, 0);
                CrossHair.GetComponent<SpriteRenderer>().color = owner.IsSuper ? superAimColor : normalAimColor;
            }
            else
            {
                CrossHair.transform.parent = transform;
                CrossHair.SetActive(false);
            }
        }
        else
        {
            CrossHair.transform.parent = transform;
            CrossHair.SetActive(false);
        }
    }


    public override void Update()
    {
        base.Update();
        if (isBallActive)
        {
            SetTrajectory();
        }
        if (owner != null && owner.mainTarget != null)
        {
            if(owner.IsAiming && owner.mainTarget != null)
            {
                currentTarget = owner.mainTarget.gameObject;
            }
        }

    }
    public void SetTrajectory()
    {
        
        if (currentTarget != null)
        {
            // Get the current position of the object
            Vector2 currentPosition = _rb.position;

            // Get the Y position of the target
            float targetY = currentTarget.transform.position.y + .5f;

            // Calculate the direction along Y axis only
            float directionY = targetY - currentPosition.y;

            // Apply velocity in the Y axis towards the target
            _rb.velocity = new Vector2(_rb.velocity.x, directionY * (currentPower / 3));
        }
    }


    private void OnDestroy()
    {
        Destroy(CrossHair);
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
