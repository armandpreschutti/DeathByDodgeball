using System;
using UnityEngine;

public class BaseBallManager : MonoBehaviour
{
    public bool hasOwner;
    public PlayerStateMachine owner;
    public int owningTeam;
    public bool isEquipped;
    public bool isBallActive;
    public bool isSuperBall;
    public float currentPower;
    public Vector2 originPoint;
    public Vector2 currentDirection;

    [Header("Components")]
    public Rigidbody2D _rb;

    [Header("VFX")]
    public GameObject _hitVfx;
    public GameObject _explosionVfx;
    public ParticleSystem _normalTrail;
    public ParticleSystem _superTrail;

    public float _knockBackPower;
    public static Action onHit;
    public static Action onExplosion;

    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    protected virtual void HandleCollision(Collider2D collision)
    {
        // Default behavior for collision
        // Can be overridden in derived classes
    }

    public virtual void EquiptBall(PlayerStateMachine stateMachine)
    {
        hasOwner = true;
        owner = stateMachine;
        stateMachine.EquipBall(gameObject);
        isEquipped = true;
        owningTeam = stateMachine.GetComponent<PawnManager>().teamId;
        isBallActive = false;
        SetBallTrailVFX(false);
        isSuperBall = false;
        _rb.velocity = Vector2.zero;
        currentDirection = Vector2.zero;
        originPoint = stateMachine.transform.position;
    }

    public virtual void Launch(bool value, Vector2 direction, bool super, float power)
    {
        currentPower = power;
        isBallActive = value;
        isSuperBall = super;
        currentDirection = direction;
        SetBallTrailVFX(true);
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

    public virtual void SelfDestruct()
    {
        Debug.Log("Ball has self-destructed");
        owner.IsDead = true;
        owner.Rb.AddForce(new Vector2(owner.transform.position.x - transform.position.x, 0).normalized * currentPower, ForceMode2D.Impulse);
        isBallActive = false;
        Instantiate(_explosionVfx, transform.position, Quaternion.identity, null);
        onExplosion?.Invoke();
        Destroy(gameObject);
    }
}