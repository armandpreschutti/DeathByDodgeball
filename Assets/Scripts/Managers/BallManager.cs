using System;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    protected PlayerStateMachine owner;
    [HideInInspector] public bool hasOwner;
    [HideInInspector] public int owningTeam;
    protected bool isEquipped;
    public bool isBallActive;
    [HideInInspector] public bool isSuperBall;
    protected float currentPower;
    protected Vector2 originPoint;
    protected Vector2 currentDirection;
    public GameObject superStateVFX;
    [Header("Components")]
    protected Rigidbody2D _rb;
    protected AudioSource _audio;
    public static Action onHit;
    public static Action onExplosion;
    public Color normalAimColor;
    public Color superAimColor;
    public static Action<int> onCaught;
    public static Action<int> onPlayerHit;
    public static Action<int> onPlayerKill;
    public static Action<int> onPlayerThrowAttempt;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _audio = GetComponent<AudioSource>();
    }
    public void Update()
    {
        SetAimIndicator();
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
                        onCaught?.Invoke(stateMachine.GetComponent<PawnManager>().slotId);
                        EquiptBall(stateMachine);
                    }
                    else if (!stateMachine.IsDead && stateMachine.GetComponent<PawnManager>().teamId != owningTeam && !stateMachine.IsInvicible)
                    {
                        PawnCollision(stateMachine);
                        onPlayerHit?.Invoke(owner.GetComponent<PawnManager>().slotId);                       

                    }
                }
            }
        }
        else if(collision.GetComponent<BallManager>() != null && isBallActive)
        {
            BallManager ballManager = collision.GetComponent<BallManager>();
            if(collision.GetComponent<BallManager>().isBallActive)
            {
                BallCollision(ballManager);
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

    }

    public virtual void Launch(bool value, Vector2 direction, bool super, float power)
    {
        currentPower = power;
        isBallActive = value;
        isSuperBall = super;
        currentDirection = direction;
        originPoint = transform.position;
        onPlayerThrowAttempt?.Invoke(owner.GetComponent<PawnManager>().slotId);
    }

    public virtual void SetBallSuperState(bool value)
    {
        superStateVFX.SetActive(value);
    }

    public virtual void PawnCollision(PlayerStateMachine stateMachine)
    {
        
    }

    public virtual void BallCollision(BallManager ballManager)
    {

    }

    public virtual void SelfDestruct()
    {

    }

    public virtual void SetAimIndicator()
    {

    }

}
