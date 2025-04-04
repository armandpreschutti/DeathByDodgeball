using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class PlayerStateMachine : MonoBehaviour
{
    [Header("States")]
    [SerializeField] PlayerBaseState _currentState;
    [SerializeField] PlayerStateFactory _states;

    [Header("Components")]
    [SerializeField] Collider2D _col;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Animator _baseAnim;
    [SerializeField] Animator _skinAnim;

    [Header("General")]
    [SerializeField] GameObject _equippedBall = null;
    [SerializeField] GameObject _closestBall = null;
    [SerializeField] Transform _holdPosition;
    [SerializeField] Transform _holdRightPosition;
    [SerializeField] Transform _holdLeftPosition;

    [SerializeField] bool _isFacingLeft;
    [SerializeField] bool _canRespawn;
    [SerializeField] float _respawnDelay;
    [SerializeField] bool _isSuper;
    [SerializeField] bool _canCatch;
    [SerializeField] bool _isNoMansArea;

    [Header("Melee")]
    [SerializeField] Vector3 _aimDirection;
    //[SerializeField] GameObject _currentTarget;
    [SerializeField] Transform _catchArea;
    [SerializeField] float _minThrowPower = 4f;
    [SerializeField] float _maxThrowPower = 20f;
    [SerializeField] float _superThrowPower = 35;
    [SerializeField] float _currentThrowPower = 4f;
    [SerializeField] float _throwPowerIncreaseRate = 7.5f;

    [Header("Locomotion")]
    [SerializeField] Vector2 _moveDirection = Vector2.zero;
    [SerializeField] float _currentSpeed;
    [SerializeField] float _moveSpeed = 4f;
    [SerializeField] float _aimSpeed = 2f;
    [SerializeField] Vector2 _dodgeDirection;
    [SerializeField] float _dodgeSpeed;

    [Header("State Variables")]
    public string CurrentSuperState;
    public string CurrentSubState;
    [SerializeField] bool _isDead;
    [SerializeField] bool _isDodging;
    [SerializeField] bool _isEquipped;
    [SerializeField] bool _isHurt;
    [SerializeField] bool _isAiming;
    [SerializeField] bool _isThrowing;
    [SerializeField] bool _isCatching;
    [SerializeField] bool _isExhausted;
    [SerializeField] bool _isInvicible;



    [Header("Input Booleans")]
    [SerializeField] Vector2 _moveInput;
    [SerializeField] Vector2 _aimInput;
    [SerializeField] bool _isThrowPressed;
    [SerializeField] bool _isCatchPressed;
    [SerializeField] bool _isDodgePressed;

    public Action<bool> OnDeath;
    public Action<bool> OnMove;
    public Action<bool> OnDodge;
    public Action<bool> OnHurt;
    public Action<bool> OnAim;
    public Action<bool> OnThrow;
    public Action<bool> OnCatch;

    public Action OnRespawn;
    public Action<bool> OnSuperState;
    public Action<bool> OnExhausted;
    public Action<bool> OnEnergized;
    public Action OnBallCaught;
    public Action OnBallContact;
    public Action OnHeal;


    public static Action<int> onDodged;

    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    public Collider2D Col { get { return _col;} set { _col = value; } }
    public SpriteRenderer SpriteRenderer { get { return _spriteRenderer;} set { _spriteRenderer = value; } }
    public Animator BaseAnim { get { return _baseAnim;} set { _baseAnim = value; } }
    public Animator SkinAnim { get { return _skinAnim; } set { _skinAnim = value; } }
    public Rigidbody2D Rb { get { return _rb; } set { _rb = value; } }  

    public GameObject EquippedBall { get {return _equippedBall;} set { _equippedBall = value; } }
    public GameObject ClosestBall { get { return _closestBall; } set { _closestBall = value; } }
    public Transform HoldPosition { get { return _holdPosition; } set { _holdPosition = value; } }
    public Transform HoldRightPosition { get { return _holdRightPosition; } set { _holdRightPosition = value; } }
    public Transform HoldLeftPosition { get { return _holdLeftPosition; } set { _holdLeftPosition = value; } }
    
   // public bool IsFacingLeft { get { return _isFacingLeft; } }
    public bool CanRespawn { get { return _canRespawn; } set { _canRespawn = value; } }
    public float RespawnDelay { get { return _respawnDelay; } set { _respawnDelay = value; } }
    public bool IsSuper { get { return _isSuper; } set { _isSuper= value; } }
    public bool CanCatch { get { return _canCatch; } set { _canCatch = value; } }
    public bool IsNoBuildArea { get { return _isNoMansArea; } set { _isNoMansArea = value; } }


    public Vector3 AimDirection { get { return _aimDirection; } set { _aimDirection = value; } }
    //public GameObject CurrentTarget { get { return _currentTarget; } set { _currentTarget = value; } }
    public Transform CatchArea { get { return _catchArea; } set { _catchArea = value; } }
    public float MinThrowPower { get { return _minThrowPower; } set { _minThrowPower = value; } }
    public float MaxThrowPower { get { return _maxThrowPower; } set { _maxThrowPower = value; } }
    public float SuperThrowPower { get { return _superThrowPower; } set { _superThrowPower = value; } }
    public float CurrentThrowPower { get { return _currentThrowPower; } set { _currentThrowPower = value; } }
    public float ThrowPowerIncreaseRate { get { return _throwPowerIncreaseRate; } set { _throwPowerIncreaseRate = value; } }

    public Vector2 MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }
    public float MoveSpeed { get {return _moveSpeed;} set { _moveSpeed = value; } } 
    public float AimSpeed { get {return _aimSpeed;} set { _aimSpeed = value; } }
    public Vector2 DodgeDirection { get { return _dodgeDirection; } set { _dodgeDirection = value; } }
    public float DodgeSpeed { get {return _dodgeSpeed;} set { _dodgeSpeed = value; } }

    public bool IsDead { get { return _isDead; } set { _isDead = value; } }
    public bool IsDodging { get {return _isDodging;} set { _isDodging = value; } }
    public bool IsEquipped { get { return _isEquipped; } set { _isEquipped = value; } }
    public bool IsHurt { get { return _isHurt; } set { _isHurt = value; } }
    public bool IsAiming { get { return _isAiming; } set { _isAiming = value; } }
    public bool IsThrowing { get { return _isThrowing; } set { _isThrowing= value; } }
    public bool IsCatching { get { return _isCatching; } set { _isCatching = value; } }
    public bool IsExhausted { get { return _isExhausted; } set { _isExhausted = value; } }
    public bool IsInvicible { get { return _isInvicible; } set { _isInvicible = value; } }
    public GameObject[] currentTargets = new GameObject[2];
    public Transform mainTarget;

    public Vector2 AimInput { get { return _aimInput; } set { _aimInput = value; } }
    public Vector2 MoveInput { get { return _moveInput; } set { _moveInput = value; } }
    public bool IsThrowPressed { get { return _isThrowPressed; } set { _isThrowPressed = value; } }
    public bool IsCatchPressed { get { return _isCatchPressed; } set { _isCatchPressed = value ; } }
    public bool IsDodgePressed { get { return _isDodgePressed; } set { _isDodgePressed = value ; } }

    private void Awake()
    {
        _states = new PlayerStateFactory(this);
        _currentState = _states.Active();
        _currentState.EnterState();

    }

    void Start()
    {
        SetPlayerInitialVariables();
       
    }

    void Update()
    {
        _moveDirection = _moveInput;
        _baseAnim.SetFloat("MoveX", _moveDirection.x);
        _baseAnim.SetFloat("MoveY", _moveDirection.y);
        _skinAnim.SetFloat("MoveX", _moveDirection.x);
        _skinAnim.SetFloat("MoveY", _moveDirection.y);
        _currentState.UpdateStates();
        SetBallEquippedPosition(_equippedBall);
        SetPlayerOrientation();
        GetTargets();
    }

    private void FixedUpdate()
    {
        _currentState.FixedUpdateStates();
    }

    public void BroadcastDodge()
    {
        onDodged?.Invoke(GetComponent<PawnManager>().slotId);
    }
    private void GetTargets()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // Ensure "currentTargets" is defined as a fixed-size array somewhere in your script
        // Example: public GameObject[] currentTargets = new GameObject[maxTargets];
        int targetIndex = 0;

        for (int i = 0; i < players.Length; i++)
        {
            // Validate that the player reference is not null
            if (players[i] != null)
            {
                PawnManager targetPawnManager = players[i].GetComponent<PawnManager>();
                PawnManager thisPawnManager = GetComponent<PawnManager>();

                if (targetPawnManager != null && thisPawnManager != null)
                {
                    PlayerStateMachine targetStateMachine = targetPawnManager.GetComponent<PlayerStateMachine>();
                    // Compare team IDs to check if the player is an enemy
                    if (targetPawnManager.teamId != thisPawnManager.teamId && !targetStateMachine.IsDead && !targetStateMachine.IsInvicible)
                    {
                        if (targetIndex < currentTargets.Length)
                        {
                            currentTargets[targetIndex] = players[i];
                            targetIndex++;
                        }
                        else
                        {
                            Debug.LogWarning("CurrentTargets array is full, cannot add more targets.");
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"PawnManager missing on {players[i].name} or on this object.");
                }
            }
            else
            {
                Debug.LogWarning("A null player reference was encountered.");
            }
        }

        // Optional: Clear any unused slots in the currentTargets array
        for (int i = targetIndex; i < currentTargets.Length; i++)
        {
            currentTargets[i] = null;
        }
        mainTarget = GetClosestTargetToPlayerAndCenter(0.5f, 0.5f);

    }

    private Transform GetClosestTargetToPlayerAndCenter(float weightY, float weightPlayer)
    {
        Transform closest = null;
        float minScore = Mathf.Infinity;

        for (int i = 0; i < currentTargets.Length; i++)
        {
            if (currentTargets[i] != null)
            {
                // Get the collider's center position
                Vector3 targetCenter = currentTargets[i].transform.position;

                // Calculate y-axis distance and player distance
                float distanceY = Mathf.Abs(transform.position.y - targetCenter.y);
                float distanceToPlayer = Vector3.Distance(transform.position, currentTargets[i].transform.position);

                // Combine the two distances into a single score using weights
                float score = (distanceY * weightY) + (distanceToPlayer * weightPlayer);

                // Check if this object is valid and has a lower score (closer) than the current closest target
                var playerStateMachine = currentTargets[i].GetComponent<PlayerStateMachine>();
                if (score < minScore && playerStateMachine != null/* && !playerStateMachine.IsDead*/)
                {
                    minScore = score;
                    closest = currentTargets[i].transform;
                }
            }
           
        }

        return closest;
    }

    public void EquipBall(GameObject ball)
    {
        if(_equippedBall == null)
        {
            ball.transform.parent = transform;
            ball.transform.position = _holdRightPosition.position;
            _equippedBall = ball;
            _isEquipped = true;
            _isSuper = false;
        }
        else
        {
            return;
        }        
    }

    public void UnequipBall(GameObject ball)
    {
        if (_equippedBall != null)
        {
            ball.transform.parent = null;
            _equippedBall = null;
            _isEquipped = false;
        }
        else
        {
            return;
        }
    }
    public void SelfDestruct()
    {
        _equippedBall.GetComponent<BallManager>().SelfDestruct();
        UnequipBall(_equippedBall);
    }
    public void DestroyBall()
    {
        if(_equippedBall !=null)
        {
            Destroy(_equippedBall);
            _equippedBall = null;
            _isEquipped = false;
        }
        else
        {
            return;
        }
    }

    public void SetPlayerOrientation()
    {
        if(_moveInput != Vector2.zero)
        {
            if (_isAiming || _isThrowing || _isCatching)
            {
                _isFacingLeft = transform.position.x > 0 ? true : false;
                _spriteRenderer.flipX = _isFacingLeft;
                _holdPosition = _isFacingLeft ? _holdRightPosition : _holdLeftPosition;
            }
            else
            {
                _isFacingLeft = _moveInput.x > 0 ? true : false;
                _spriteRenderer.flipX = !_isFacingLeft;
                _holdPosition = _isFacingLeft ? _holdLeftPosition : _holdRightPosition;
            }
        }
        else
        {
            _isFacingLeft = transform.position.x > 0 ? true : false;
            _spriteRenderer.flipX = _isFacingLeft;
            _holdPosition = _isFacingLeft ? _holdRightPosition : _holdLeftPosition;
        }
    }

    public void ThrowBall()
    {
        if(_equippedBall!= null)
        {
            _equippedBall.GetComponent<BallManager>().Launch(true, _aimDirection, _currentThrowPower <= _maxThrowPower ? false : true/*, _currentTarget*/, CurrentThrowPower);
            UnequipBall(_equippedBall);
        }

    }

    public void SetBallEquippedPosition(GameObject ball)
    {
        if(ball != null)
        {
            ball.transform.position = _holdPosition.position;
        }
    }

    public void SetPlayerInitialVariables()
    {
        _col = GetComponent<Collider2D>();
        _currentThrowPower = _minThrowPower;
        _isDead = false;
        _isDodging =false;
        _isEquipped =false;
        _isHurt =false;
        _isAiming =false;
        _isThrowing =false;
        _isCatching =false;
    }
    public void Animation_DodgeEnd()
    {
        _isDodging = false;
    }

    public void Animation_CatchEnd()
    {
        _isCatching = false;
    }

    public void Animation_ThrowEnd()
    {
        _isThrowing = false;
    }

    
}
