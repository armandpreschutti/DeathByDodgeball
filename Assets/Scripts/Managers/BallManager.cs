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
    protected PlayerStateMachine owner;
    [HideInInspector] public bool hasOwner;
    [HideInInspector] public int owningTeam;
    protected bool isEquipped;
    protected bool isBallActive;
    protected bool isSuperBall;
    protected float currentPower;
    protected Vector2 originPoint;
    protected Vector2 currentDirection;
    public GameObject superStateVFX;
  
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
        
        if (transform.parent != null && transform.parent.TryGetComponent(out BallSpawnHandler ballSpawnHandler))
        {
            ballSpawnHandler.SpawnNewBall();
        }
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

    public virtual void SetBallSuperState(bool value)
    {
        superStateVFX.SetActive(value);
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
