using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BallManager : MonoBehaviour
{
    public bool hasOwner;
    public PlayerStateMachine owner;
    public int owningTeam;
    public bool isEquipped;
    public bool isBallActive;
    public bool isSuperBall;
    public GameObject target;
    public float currentPower;
    public Vector2 originPoint;
    public Vector2 currentDirection;


    [Header("Components")]
    public Collider2D _col;
    public Rigidbody2D _rb;
    public SpriteRenderer _spriteRenderer;

    [Header("VFX")]
    public GameObject _hitVfx;
    public GameObject _explosionVfx;
    public Sprite _ballSprite;
    public Sprite _bombSprite;
    public ParticleSystem _normalTrail;
    public ParticleSystem _superTrail;

    public float _knockBackPower;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerStateMachine>() != null)
        {
            PlayerStateMachine stateMachine = collision.GetComponent<PlayerStateMachine>();
            if (!hasOwner)
            {
                if (!stateMachine.IsEquipped)
                {
                    EquiptBall(stateMachine);
                }
            }
            else
            {
                if (isBallActive && stateMachine.GetComponent<PawnManager>().teamId != owningTeam)
                {
                    if (stateMachine.IsCatching)
                    {
                        EquiptBall(stateMachine);
                    }
                    else if (!stateMachine.IsDead)
                    {
                        stateMachine.IsDead = true;
                        isBallActive = false;
                        if (isSuperBall)
                        {
                            Instantiate(_explosionVfx, transform.position, Quaternion.identity,null );
                            Destroy(gameObject);

                        }
                        else
                        {
                            Instantiate(_hitVfx, transform.position, Quaternion.identity, null);
                            _rb.velocity = new Vector2(_rb.velocity.x, originPoint.y < transform.position.y ? 7.5f : -7.5f);
                            Destroy(gameObject, 1f);
                        }

                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (isBallActive)
        {
            SetTrajectory();
        }
    }

    public void EquiptBall(PlayerStateMachine stateMachine)
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
        //target = null;
        currentDirection = Vector2.zero;
        originPoint = stateMachine.transform.position;
    }

    public void Launch(bool value, Vector2 direction, bool super, GameObject playerTarget, float power)
    {
        currentPower = power;
        target = playerTarget;
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

    public void SetTrajectory()
    {
        if (target != null)
        {
            // Get the current position of the object
            Vector2 currentPosition = _rb.position;

            // Get the Y position of the target
            float targetY = target.transform.position.y + 0.4f;

            // Calculate the direction along Y axis only
            float directionY = targetY - currentPosition.y;

            // Apply velocity in the Y axis towards the target
            _rb.velocity = new Vector2(_rb.velocity.x, directionY * (currentPower / 3));
        }
    }
    
}
