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

    [Header("Components")]
    public Collider2D _col;
    public Rigidbody2D _rb;
    public SpriteRenderer _spriteRenderer;

    [Header("VFX")]
    public GameObject _explosionImpact;
    public Sprite _ballSprite;
    public Sprite _bombSprite;
    public ParticleSystem _normalTrail;
    public ParticleSystem _superTrail;

    [Header("Variables")]
    public Transform _parent;
    public float _ballDamage;
    public float _knockBackPower;
    public Vector2 Trajectory;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    /*    private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerStateMachine>() != null)
            {
                PlayerStateMachine stateMachine = collision.gameObject.GetComponent<PlayerStateMachine>();
                if (!hasOwner)
                {
                    if (!stateMachine.IsEquipped)
                    {
                        EquiptBall(stateMachine);
                    }
                }
                else
                {
                    if (stateMachine != owner && isBallActive && stateMachine.GetComponent<PawnManager>().teamId != owningTeam)
                    {
                        if (stateMachine.IsCatching)
                        {
                            EquiptBall(stateMachine);
                        }
                        else if (!stateMachine.IsDead)
                        {
                            stateMachine.IsDead = true;
                            Destroy(gameObject);
                        }

                    }
                }
            }
        }*/
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
                if (/*stateMachine != owner &&*/ isBallActive && stateMachine.GetComponent<PawnManager>().teamId != owningTeam)
                {
                    if (stateMachine.IsCatching)
                    {
                        EquiptBall(stateMachine);
                    }
                    else if (!stateMachine.IsDead)
                    {
                        stateMachine.IsDead = true;
                        isBallActive = false;

                        _rb.velocity = new Vector2(_rb.velocity.x, originPoint.y < transform.position.y ? 7.5f : -7.5f); 
                        Destroy(gameObject, 1f);
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
        target = null;
        currentPower = 0.0f;
        originPoint = stateMachine.transform.position;
    }

    public void Launch(bool value, Vector2 direction, bool super, GameObject playerTarget, float power)
    {
        currentPower = power;
        target = playerTarget;
        isBallActive = value;
        isSuperBall = super;
        _rb.AddForce(direction, ForceMode2D.Impulse);
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
            float targetY = target.transform.position.y;

            // Calculate the direction along Y axis only
            float directionY = targetY - currentPosition.y;

            // Apply velocity in the Y axis towards the target
            _rb.velocity = new Vector2(_rb.velocity.x, directionY * (currentPower /3));
        }
    }
}
