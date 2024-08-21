using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] Animator _anim;
    [SerializeField] PlayerInput _playerInput;

    [Header("General")]
    [SerializeField] GameObject _equippedBall = null;
    [SerializeField] Transform _holdPosition;
    [SerializeField] Transform _holdRightPosition;
    [SerializeField] Transform _holdLeftPosition;

    [Header("Melee")]
    [SerializeField] Vector3 _aimDirection;
    [SerializeField] float _minThrowPower = 4f;
    [SerializeField] float _maxThrowPower = 20f;
    [SerializeField] float _superThrowPower = 35;
    [SerializeField] float _currentThrowPower= 4f;
    [SerializeField] float _throwPowerIncreaseRate = 7.5f;

    [Header("Locomotion")]
    [SerializeField] Vector2 _moveDirection = Vector2.zero;
    [SerializeField] float _currentSpeed;
    [SerializeField] float _moveSpeed = 4f;
    [SerializeField] float _aimSpeed = 2f;
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

    [Header("Input Booleans")]
    [SerializeField] Vector2 _moveInput;
    [SerializeField] Vector2 _aimInput;
    [SerializeField] bool _isThrowPressed;
    [SerializeField] bool _isCatchPressed;
    [SerializeField] bool _isDodgePressed;

    [SerializeField] event Action _onDeath;
    [SerializeField] event Action _onDodge;
    [SerializeField] event Action _onEquip;
    [SerializeField] event Action _onHurt;
    [SerializeField] event Action _onAim;
    [SerializeField] event Action _onThrow;
    [SerializeField] event Action _onCatch;
    [SerializeField] event Action _onRespawn;
    

    public Action OnDeath { get { return _onDeath; } set { _onDeath = value; } }
    public Action OnDodge { get { return _onDodge; } set { _onDodge = value; } }
    public Action OnEquip { get { return _onEquip; } set { _onEquip = value; } }
    public Action OnHurt { get { return _onHurt; } set { _onHurt = value; } }
    public Action OnAim { get { return _onAim; } set { _onAim = value; } }
    public Action OnThrow { get { return _onThrow; } set { _onThrow = value; } }
    public Action OnCatch { get { return _onCatch; } set { _onCatch = value; } }
    public Action OnRespawn { get { return _onRespawn; } set { _onRespawn = value; } }

    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    public Collider2D Col { get { return _col;} set { _col = value; } }
    public SpriteRenderer SpriteRenderer { get { return _spriteRenderer;} set { _spriteRenderer = value; } }
    public Animator Anim { get { return _anim;} set { _anim = value; } }
    public PlayerInput PlayerInput { get { return _playerInput; } set { _playerInput = value; } }

    public GameObject EquippedBall { get {return _equippedBall;} set { _equippedBall = value; } }
    public Transform HoldPosition { get { return _holdPosition; } set { _holdPosition = value; } }
    public Transform HoldRightPosition { get { return _holdRightPosition; } set { _holdRightPosition = value; } }
    public Transform HoldLeftPosition { get { return _holdLeftPosition; } set { _holdLeftPosition = value; } }

    public Vector3 AimDirection { get { return _aimDirection; } set { _aimDirection = value; } }
    public float MinThrowPower { get { return _minThrowPower; } set { _minThrowPower = value; } }
    public float MaxThrowPower { get { return _maxThrowPower; } set { _maxThrowPower = value; } }
    public float SuperThrowPower { get { return _superThrowPower; } set { _superThrowPower = value; } }
    public float CurrentThrowPower { get { return _currentThrowPower; } set { _currentThrowPower = value; } }
    public float ThrowPowerIncreaseRate { get { return _throwPowerIncreaseRate; } set { _throwPowerIncreaseRate = value; } }

    public Vector2 MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }
    public float MoveSpeed { get {return _moveSpeed;} set { _moveSpeed = value; } } 
    public float AimSpeed { get {return _aimSpeed;} set { _aimSpeed = value; } }
    public float DodgeSpeed { get {return _dodgeSpeed;} set { _dodgeSpeed = value; } }

    public bool IsDead { get { return _isDead; } set { _isDead = value; } }
    public bool IsDodging { get {return _isDodging;} set { _isDodging = value; } }
    public bool IsEquipped { get { return _isEquipped; } set { _isEquipped = value; } }
    public bool IsHurt { get { return _isHurt; } set { _isHurt = value; } }
    public bool IsAiming { get { return _isAiming; } set { _isAiming = value; } }
    public bool IsThrowing { get { return _isThrowing; } set { _isThrowing= value; } }
    public bool IsCatching { get { return _isCatching; } set { _isCatching = value; } }

    public Vector2 AimInput { get { return _aimInput; } set { _aimInput = value; } }
    public Vector2 MoveInput { get { return _moveInput; } set { _moveInput = value; } }
    public bool IsThrowPressed { get { return _isThrowPressed; } set { _isThrowPressed = value; } }
    public bool IsCatchPressed { get { return _isCatchPressed; } set { _isCatchPressed = value ; } }
    public bool IsDodgePressed { get { return _isDodgePressed; } set { _isDodgePressed = value ; } }

    private void Awake()
    {
        SetPlayerComponents();

        _states = new PlayerStateFactory(this);
        _currentState = _states.Unequipped();
        _currentState.EnterState();
    }
    void Start()
    {
        SetPlayerInitialVariables();
        SetPlayerDirection();
    }

    void Update()
    {
        _moveDirection = _moveInput;
        _anim.SetFloat("MoveX", _moveDirection.x);
        _anim.SetFloat("MoveY", _moveDirection.y);
        _currentState.UpdateStates();
    }

    private void FixedUpdate()
    {
        _currentState.FixedUpdateStates();
    }

    public void SetPlayerComponents()
    {
        _playerInput = GetComponentInParent<PlayerInput>();
        _col = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    public void EquipBall(GameObject ball)
    {
        if(_equippedBall == null)
        {
            ball.transform.parent = transform;
            ball.transform.position = _holdRightPosition.position;
            _equippedBall = ball;
            _onEquip?.Invoke();
            _isEquipped = true;
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

    public void ChangeBallPosition(GameObject ball)
    {
        if(_holdPosition != null)
        {
            ball.transform.position = _holdPosition.position;
        }
    }

    public void SetPlayerInitialVariables()
    {
        _currentThrowPower = _minThrowPower;
        _isDead = false;
        _isDodging =false;
        _isEquipped =false;
        _isHurt =false;
        _isAiming =false;
        _isThrowing =false;
        _isCatching =false;
    }
    public void SetPlayerDirection()
    {
        bool flipped = transform.position.x > 0f;

        GetComponent<SpriteRenderer>().flipX = flipped;
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
