using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal;
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
    public float currentPower;
    public Vector2 originPoint;
    public Vector2 currentDirection;
    public GameObject _hitVfx;

    [Header("Components")]
    protected Rigidbody2D _rb;
    public static Action onHit;
    public static Action onExplosion;

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
                if (isBallActive)
                {
                    if (stateMachine.IsCatching)
                    {
                        stateMachine.OnBallCaught?.Invoke();
                        EquiptBall(stateMachine);
                    }
                    else if (!stateMachine.IsDead && stateMachine.GetComponent<PawnManager>().teamId != owningTeam)
                    {
                        PawnCollision(stateMachine);

                    }
                }
            }
        }
        else if(collision.GetComponent<BallManager>() != null && isBallActive)
        {
            if(collision.GetComponent<BallManager>().isBallActive)
            {
                BallCollision();
            }

        }
    }

    public virtual void EquiptBall(PlayerStateMachine stateMachine)
    {
        hasOwner = true;
        owner = stateMachine;
        stateMachine.EquipBall(gameObject);
        isEquipped = true;
        owningTeam = stateMachine.GetComponent<PawnManager>().teamId;
        isBallActive = false;
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
    }

    public virtual void PawnCollision(PlayerStateMachine stateMachine)
    {
        
    }

    public virtual void BallCollision()
    {

    }

    public virtual void SelfDestruct()
    {

    }


}
