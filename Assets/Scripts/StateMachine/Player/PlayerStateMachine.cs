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
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Animator _anim;
    [SerializeField] PlayerInput _playerInput;

    [Header("General")]
    [SerializeField] GameObject _equippedBall = null;
    [SerializeField] GameObject _aimingObject = null;
    [SerializeField] Transform _holdPosition;
    [SerializeField] Transform _holdRightPosition;
    [SerializeField] Transform _holdLeftPosition;
    [SerializeField] Slider _playerThrowBar;
    [SerializeField] Slider _playerStaminaBar;

    [Header("Melee")]
    [SerializeField] Vector3 _aimDirection;
    [SerializeField] float _throwDuration = .15f;
    [SerializeField] float _minThrowPower = 4f;
    [SerializeField] float _maxThrowPower = 20f;
    [SerializeField] float _superThrowPower = 35;
    [SerializeField] float _currentThrowPower= 4f;
    [SerializeField] float _throwPowerIncreaseRate = 7.5f;
    [SerializeField] float _catchDuration = .5f;
    [SerializeField] float _hurtDuration;

    [Header("Locomotion")]
    [SerializeField] Vector2 _moveDirection = Vector2.zero;
    [SerializeField] float _currentSpeed;
    [SerializeField] float _idleSpeed = 4f;
    [SerializeField] float _aimSpeed = 2f;
    [SerializeField] float _dodgeSpeed;
    [SerializeField] float _dodgeDuration;
    [SerializeField] float _staminaRefillRate;
    [SerializeField] float _currentStamina;
    [SerializeField] float _maxStamina;


    [Header("State Booleans")]
    [SerializeField] bool _isMobile;
    [SerializeField] bool _isInvicible;
    [SerializeField] bool _isDead;
    [SerializeField] bool _isDodging;
    [SerializeField] bool _isEquipped;
    [SerializeField] bool _isHurt;
    [SerializeField] bool _isAiming;
    [SerializeField] bool _isThrowing;
    [SerializeField] bool _isCatching;
    [SerializeField] bool _isExhausted;

    [Header("Input")]
    [SerializeField] InputAction _moveAction;
    [SerializeField] InputAction _throwAction;
    [SerializeField] InputAction _catchAction;
    [SerializeField] InputAction _dodgeAction;
    [SerializeField] InputAction _aimAction;

    [Header("Input Booleans")]
    [SerializeField] bool _isMovePressed;
    [SerializeField] bool _isAimPressed;
    [SerializeField] bool _isCatchPressed;
    [SerializeField] bool _isDodgePressed;
    [SerializeField] bool _isReadyPressed;

    [SerializeField] event Action _onDeath;
    [SerializeField] event Action _onDodge;
    [SerializeField] event Action _onStaminaRefilled;
    [SerializeField] event Action _onStaminaReplenish;
    [SerializeField] event Action _onStaminaDepleted;
    [SerializeField] event Action _onEquip;
    [SerializeField] event Action _onHurt;
    [SerializeField] event Action _onAim;
    [SerializeField] event Action _onThrow;
    [SerializeField] event Action _onCatch;
    [SerializeField] event Action _onRespawn;
    

    public Action OnDeath { get { return _onDeath; } set { _onDeath = value; } }
    public Action OnDodge { get { return _onDodge; } set { _onDodge = value; } }
    public Action OnStaminaFilled { get { return _onStaminaRefilled; } set { _onStaminaRefilled = value; } }
    public Action OnStaminaReplenish { get { return _onStaminaReplenish; } set { _onStaminaReplenish = value; } }
    public Action OnStaminaDepleted { get { return _onStaminaDepleted; } set { _onStaminaDepleted = value; } }
    public Action OnEquip { get { return _onEquip; } set { _onEquip = value; } }
    public Action OnHurt { get { return _onHurt; } set { _onHurt = value; } }
    public Action OnAim { get { return _onAim; } set { _onAim = value; } }
    public Action OnThrow { get { return _onThrow; } set { _onThrow = value; } }
    public Action OnCatch { get { return _onCatch; } set { _onCatch = value; } }
    public Action OnRespawn { get { return _onRespawn; } set { _onRespawn = value; } }

    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    public Collider2D Col { get { return _col;} set { _col = value; } }
    public Rigidbody2D Rb { get { return _rb;} set { _rb = value; } }
    public SpriteRenderer SpriteRenderer { get { return _spriteRenderer;} set { _spriteRenderer = value; } }
    public Animator Anim { get { return _anim;} set { _anim = value; } }
    public PlayerInput PlayerInput { get { return _playerInput; } set { _playerInput = value; } }

    public GameObject EquippedBall { get {return _equippedBall;} set { _equippedBall = value; } }
    public GameObject AimingObject { get { return _aimingObject; } set { _aimingObject = value; } }
    public Transform HoldPosition { get { return _holdPosition; } set { _holdPosition = value; } }
    public Transform HoldRightPosition { get { return _holdRightPosition; } set { _holdRightPosition = value; } }
    public Transform HoldLeftPosition { get { return _holdLeftPosition; } set { _holdLeftPosition = value; } }
    public Slider PlayerThrowBar { get { return _playerThrowBar; } set { _playerThrowBar = value; } }
    public Slider PlayerStaminaBar { get { return _playerStaminaBar; } set { _playerStaminaBar = value; } }

    public Vector3 AimDirection { get { return _aimDirection; } set { _aimDirection = value; } }
    public float ThrowDuration { get { return _throwDuration; } set { _throwDuration = value; } }
    public float MinThrowPower { get { return _minThrowPower; } set { _minThrowPower = value; } }
    public float MaxThrowPower { get { return _maxThrowPower; } set { _maxThrowPower = value; } }
    public float SuperThrowPower { get { return _superThrowPower; } set { _superThrowPower = value; } }
    public float CurrentThrowPower { get { return _currentThrowPower; } set { _currentThrowPower = value; } }
    public float ThrowPowerIncreaseRate { get { return _throwPowerIncreaseRate; } set { _throwPowerIncreaseRate = value; } }
    public float CatchDuration { get { return _catchDuration; } set { _catchDuration = value; } }
    public float HurtDuration { get { return _hurtDuration; } set { _hurtDuration = value; } }

    public Vector2 MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }
    public float IdleSpeed { get {return _idleSpeed;} set { _idleSpeed = value; } } 
    public float AimSpeed { get {return _aimSpeed;} set { _aimSpeed = value; } }
    public float DodgeSpeed { get {return _dodgeSpeed;} set { _dodgeSpeed = value; } }
    public float DodgeDuration { get {return _dodgeDuration;} set { _dodgeDuration = value; } }
    public float StaminaRefillRate { get { return _staminaRefillRate; } set { _staminaRefillRate = value; } }
    public float CurrentStamina { get {return _currentStamina;} set { _currentStamina = value; } }
    public float MaxStamina { get {return _maxStamina;} set { _maxStamina = value; } }

    public bool IsMobile { get { return _isMobile; } set { _isMobile = value; } }
    public bool IsInvicible { get { return _isInvicible; } set { _isInvicible = value; } }
    public bool IsDead { get { return _isDead; } set { _isDead = value; } }
    public bool IsDodging { get {return _isDodging;} set { _isDodging = value; } }
    public bool IsEquipped { get { return _isEquipped; } set { _isEquipped = value; } }
    public bool IsHurt { get { return _isHurt; } set { _isHurt = value; } }
    public bool IsAiming { get { return _isAiming; } set { _isAiming = value; } }
    public bool IsThrowing { get { return _isThrowing; } set { _isThrowing= value; } }
    public bool IsCatching { get { return _isCatching; } set { _isCatching = value; } }
    public bool IsExhausted { get { return _isExhausted; } set { _isExhausted = value; } }

    public Vector2 AimInput { get { return _aimAction.ReadValue<Vector2>(); } }
    public bool IsAimPressed { get { return _isAimPressed; } set { _isAimPressed = value; } }
    public bool IsCatchPressed { get { return _isCatchPressed; } set { _isCatchPressed = value ; } }
    public bool IsDodgePressed { get { return _isDodgePressed; } set { _isDodgePressed = value ; } }
    public bool IsReadyPressed { get { return _isReadyPressed; } set { _isReadyPressed = value ; } }

    private void Awake()
    {
        SetPlayerComponents();
        SetPlayerInputActions();

        _states = new PlayerStateFactory(this);
        _currentState = _states.Unequipped();
        _currentState.EnterState();
    }

    private void OnEnable()
    {
        EnableInputActions();
        _onDodge += TriggerStaminaReset;
        _onThrow += TriggerStaminaReset;
    }

    private void OnDisable()
    {
        DisableInputActions();
        _onDodge -= TriggerStaminaReset;
        _onThrow -= TriggerStaminaReset;
    }
    
    void Start()
    {
        SetPlayerInitialVariables();
        SetPlayerDirection();
    }

    void Update()
    {
        _moveDirection = _isMobile ? _moveAction.ReadValue<Vector2>() : Vector2.zero;
        _anim.SetFloat("MoveX", _moveDirection.x);
        _anim.SetFloat("MoveY", _moveDirection.y);
        MonitorStamina();
        _currentState.UpdateStates();
    }

    private void FixedUpdate()
    {
        _currentState.FixedUpdateStates();
    }

    public void StartAiming(InputAction.CallbackContext context)
    {

        if (!_isDodging
            && !_isCatching
            && _isEquipped
            && !_isHurt
            && !_isThrowing
            && _isMobile)
        {
            if(_currentStamina > _minThrowPower)
            {
                _isAimPressed = true;
            }
            else
            {
                _onStaminaDepleted?.Invoke();
            }           
        }        
    }

    public void StopAiming(InputAction.CallbackContext context)
    {

        _isAimPressed = false;
    }

    public void StartCatching(InputAction.CallbackContext context)
    {
        if (!_isDodging

            && !_isCatching
            && !_isEquipped
            && !_isHurt
            && !_isThrowing
            && _isMobile)
        {
            _isCatchPressed = true;
        }            
    }

    public void StopCatching(InputAction.CallbackContext context)
    {

        _isCatchPressed = false;
    }

    public void Dodge(InputAction.CallbackContext context)
    {
        if (!_isDodging
            && _currentStamina > 10f
            && !_isCatching
            && !_isHurt
            && !_isThrowing
            && _isMobile)
        {
            _isDodgePressed = context.ReadValueAsButton();
        }
        else if (_currentStamina < 10f)
        {
            _onStaminaDepleted?.Invoke();
        }
    }

    public void SetPlayerComponents()
    {
        _playerInput = GetComponent<PlayerInput>();
        _col = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    public void EquipBall(GameObject ball)
    {
        if(_equippedBall == null)
        {
            ball.transform.parent = transform;
            ball.GetComponent<BallStateMachine>().Parent = transform;
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
        _currentStamina = _maxStamina;
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

    public void SetPlayerInputActions()
    {
        _throwAction = _playerInput.actions["Throw"];
        _catchAction = _playerInput.actions["Catch"];
        _moveAction = _playerInput.actions["Move"];
        _dodgeAction = _playerInput.actions["Dodge"];
        _aimAction = _playerInput.actions["Aim"];
    }

    public void EnableInputActions()
    {
        _throwAction.started += StartAiming;
        _throwAction.canceled += StopAiming;
        _catchAction.performed += StartCatching;
        _dodgeAction.performed += Dodge;
    }

    public void DisableInputActions()
    {
        _throwAction.started -= StartAiming;
        _throwAction.canceled -= StopAiming;
        _catchAction.performed -= StartCatching;
        _dodgeAction.performed -= Dodge;
    }

    public void MonitorStamina()
    {
        if(_currentStamina > _maxStamina)
        {
            _currentStamina = _maxStamina;
            _onStaminaRefilled?.Invoke();
        }

        if(_currentStamina < _maxStamina && !_isAiming && !IsExhausted)
        { 
            if(_moveDirection == Vector2.zero)
            {
                _currentStamina += _staminaRefillRate;
            }
            else
            {
                _currentStamina += (_staminaRefillRate/2);
            }
            _onStaminaReplenish?.Invoke();
        }

        if(_currentStamina <= 0)
        {
            _currentStamina = 0;
        }
    }

    private void TriggerStaminaReset()
    {
        StartCoroutine(StaminaReset());
    }

    IEnumerator StaminaReset()
    {
        _isExhausted = true;
        yield return new WaitForSeconds(5f);
        _isExhausted = false;
    }
}
