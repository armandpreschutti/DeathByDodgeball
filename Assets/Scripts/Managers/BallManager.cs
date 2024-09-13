using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public static Action onHit;
    public static Action onExplosion;


    public string playerName;
    public GameObject crosshair;  // The RectTransform of your canvas UI element
    public GameObject targetObject;             // The target GameObject you want to match
    public Vector2 offset;
    public TextMeshPro aimerName;

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
                if (!stateMachine.IsEquipped && !stateMachine.IsDead)
                {
                    EquiptBall(stateMachine);
                }
            }
            else
            {
                if (isBallActive && stateMachine.GetComponent<PawnManager>().teamId != owningTeam)
                {
                    if (stateMachine.IsCatching && !isSuperBall)
                    {
                        stateMachine.OnBallCaught?.Invoke();
                        EquiptBall(stateMachine);
                    }
                    else if (!stateMachine.IsDead)
                    {
                        crosshair.gameObject.SetActive(false);
                        stateMachine.IsDead = true;
                        stateMachine.Rb.AddForce(new Vector2(stateMachine.transform.position.x - transform.position.x , 0).normalized * currentPower, ForceMode2D.Impulse);
                        isBallActive = false;
                        if (isSuperBall)
                        {
                            Instantiate(_explosionVfx, transform.position, Quaternion.identity,null );
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
                }
            }
        }
        else if(collision.GetComponent<BallManager>() != null && isBallActive)
        {
            if(collision.GetComponent<BallManager>().isBallActive)
            {
                Instantiate(_hitVfx, transform.position, Quaternion.identity, null);
                target = null;
                _rb.velocity = new Vector2(-_rb.velocity.x, originPoint.y < transform.position.y ? 7.5f : -7.5f);
                Destroy(gameObject, 3f);
            }

        }
    }

    private void FixedUpdate()
    {
        if (isBallActive)
        {
            SetTrajectory();
            SetCrosshairs();
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
        target = null;
        crosshair.gameObject.SetActive(false);
        currentDirection = Vector2.zero;
        originPoint = stateMachine.transform.position;
        playerName = owner.GetComponent<PawnManager>().playerName;
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
            float targetY = target.transform.position.y + 0.5f;

            // Calculate the direction along Y axis only
            float directionY = targetY - currentPosition.y;

            // Apply velocity in the Y axis towards the target
            _rb.velocity = new Vector2(_rb.velocity.x, directionY * (currentPower / 3));
        }
    }
    void SetCrosshairs()
    {
        //targetObject = owner.CurrentTarget;
        if (target != null)
        {
            // Set the crosshair's position on the screen
            crosshair.transform.position = target.transform.position;
            // Ensure the crosshair is visible
            crosshair.gameObject.SetActive(true);
            aimerName.text = playerName;
        }
        else
        {
            crosshair.gameObject.SetActive(false);
        }
    }
}
