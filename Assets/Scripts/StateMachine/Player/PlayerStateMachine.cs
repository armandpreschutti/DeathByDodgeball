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
    [SerializeField] Transform _holdPosition;
    [SerializeField] Transform _target;

    [Header("UI")]
    [SerializeField] Slider _dodgeSlider;
    [SerializeField] Slider _healthSlider;
    [SerializeField] Slider _playerBar;

    [Header("Health")]
    [SerializeField] float _currentHealth;
    [SerializeField] float _maxHealth = 100f;
    [SerializeField] float _minhealth = 0f;
    [SerializeField] float _hitDuration;

    [Header("Melee")]
    [SerializeField] Vector3 _aimDirection;
    [SerializeField] float _minThrowPower = 4f;
    [SerializeField] float _maxThrowPower = 20f;
    [SerializeField] float _currentThrowPower= 4f;
    [SerializeField] float _throwPowerIncreaseRate = 7.5f;
    [SerializeField] float _catchDuration = .5f;

    [Header("Locomotion")]
    [SerializeField] Vector2 _moveDirection = Vector2.zero;
    [SerializeField] float _currentSpeed;
    [SerializeField] float _idleSpeed = 4f;
    [SerializeField] float _aimSpeed = 2f;
    [SerializeField] float _dodgeSpeed;
    [SerializeField] float _dodgeDuration;
    [SerializeField] float _dodgeRefillRate;
    [SerializeField] int _totalDodges;
    [SerializeField] int _maxDodges;

    [Header("State Booleans")]
    [SerializeField] bool _isDead;
    [SerializeField] bool _isDodging;
    [SerializeField] bool _isEquipped;
    [SerializeField] bool _isHurt;
    [SerializeField] bool _isReady;
    [SerializeField] bool _isAiming;
    [SerializeField] bool _isThrowing;
    [SerializeField] bool _isCatching;

    [Header("Input")]
    [SerializeField] InputAction _moveAction;
    [SerializeField] InputAction _aimAction;
    [SerializeField] InputAction _catchAction;
    [SerializeField] InputAction _dodgeAction;


    [Header("Input Booleans")]
    [SerializeField] bool _isMovePressed;
    [SerializeField] bool _isAimPressed;
    [SerializeField] bool _isCatchPressed;
    [SerializeField] bool _isDodgePressed;
    [SerializeField] bool _isReadyPressed;

    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    public Collider2D Col { get { return _col;} set { _col = value; } }
    public Rigidbody2D Rb { get { return _rb;} set { _rb = value; } }
    public SpriteRenderer SpriteRenderer { get { return _spriteRenderer;} set { _spriteRenderer = value; } }
    public Animator Anim { get { return _anim;} set { _anim = value; } }
    public PlayerInput PlayerInput { get { return _playerInput; } set { _playerInput = value; } }

    public GameObject EquippedBall { get {return _equippedBall;} set { _equippedBall = value; } }
    public Transform HoldPosition { get { return _holdPosition; } set { _holdPosition = value; } }
    public Transform Target { get { return _target; } set { _target = value; } }

    public Slider DodgeSlider { get { return _dodgeSlider; } set { _dodgeSlider = value; } }
    public Slider HealthSlider { get { return _healthSlider; } set { _healthSlider = value; } }
    public Slider PlayerBar { get { return _playerBar; } set { _healthSlider = value; } }

    public float CurrentHealth { get {return _currentHealth; } set { _currentHealth= value; } }
    public float MaxHealth { get {return _maxHealth;} set { _maxHealth = value; } } 
    public float Minhealth { get {return _minhealth; } set { _minhealth = value; } } 
    public float HitDuration { get {return _hitDuration;} set { _hitDuration = value; } }

    public Vector3 AimDirection { get { return _aimDirection; } set { _aimDirection = value; } }
    public float MinThrowPower { get { return _minThrowPower; } set { _minThrowPower = value; } }
    public float MaxThrowPower { get { return _maxThrowPower; } set { _maxThrowPower = value; } }
    public float CurrentThrowPower { get { return _currentThrowPower; } set { _currentThrowPower = value; } }
    public float ThrowPowerIncreaseRate { get { return _throwPowerIncreaseRate; } set { _throwPowerIncreaseRate = value; } }
    public float CatchDuration { get { return _catchDuration; } set { _catchDuration = value; } }

    public Vector2 MoveDirection { get { return _moveDirection; } set { _moveDirection = value; } }
    public float IdleSpeed { get {return _idleSpeed;} set { _idleSpeed = value; } } 
    public float AimSpeed { get {return _aimSpeed;} set { _aimSpeed = value; } }
    public float DodgeSpeed { get {return _dodgeSpeed;} set { _dodgeSpeed = value; } }
    public float DodgeDuration { get {return _dodgeDuration;} set { _dodgeDuration = value; } }
    public float DodgeRefillRate { get { return _dodgeRefillRate; } set { _dodgeRefillRate = value; } }
    public int TotalDodges { get {return _totalDodges;} set { _totalDodges = value; } }
    public int MaxDodges { get {return _maxDodges;} set { _maxDodges = value; } }

    public bool IsDead { get { return _isDead; } set { _isDead = value; } }
    public bool IsDodging { get {return _isDodging;} set { _isDodging = value; } }
    public bool IsEquipped { get { return _isEquipped; } set { _isEquipped = value; } }
    public bool IsHurt { get { return _isHurt; } set { _isHurt = value; } }
    
    public bool IsAiming { get { return _isAiming; } set { _isAiming = value; } }
    public bool IsThrowing { get { return _isThrowing; } set { _isThrowing= value; } }
    public bool IsCatching { get { return _isCatching; } set { _isCatching = value; } }

    public bool IsMovePressed { get { return _isMovePressed; } set { _isMovePressed= value ; } }
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
    }

    private void OnDisable()
    {
        DisableInputActions();
    }
    
    void Start()
    {
        SetPlayerInitialVariables();
    }

    void Update()
    {
        _moveDirection = _moveAction.ReadValue<Vector2>();
        _anim.SetFloat("MoveX", _moveDirection.x);
        _anim.SetFloat("MoveY", _moveDirection.y);
        _currentState.UpdateStates();
    }

    private void FixedUpdate()
    {
        bool flipped = _target.position.x < transform.position.x;
        transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180f : 0f, 0f));
    }

    public void StartAiming(InputAction.CallbackContext context)
    {
        if (!_isDodging
            && !_isCatching
            && _isEquipped
            && !_isHurt)
        {
            _isAimPressed = true;
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
            && !_isHurt)
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
        if(!_isDodging 
            && _totalDodges > 0 
            && !_isAiming
            && !_isCatching
            && !_isHurt)
        {
            _isDodgePressed = context.ReadValueAsButton();
        }
    }

    public void Ready(InputAction.CallbackContext context)
    {
        _isReadyPressed = true;
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
            ball.transform.position = _holdPosition.position;
            _equippedBall = ball;
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

    public void TakeDamage(float damage)
    {
        _isHurt = true;
        _currentHealth -= damage;
    }
    

    public void UpdateHealthBar()
    {
        _healthSlider.value = _currentHealth;
    }

    public void SetPlayerInitialVariables()
    {
        _totalDodges = _maxDodges;
        _currentThrowPower = _minThrowPower;
        _currentHealth = _maxHealth;
        _dodgeSlider = GameObject.Find($"{name}DodgeBar").GetComponent<Slider>();
        _healthSlider = GameObject.Find($"{name}HealthBar").GetComponent<Slider>();
        _isDead = false;
        _isDodging =false;
        _isEquipped =false;
        _isHurt =false;
        _isReady =false;
        _isAiming =false;
        _isThrowing =false;
        _isCatching =false;
    }

    public void SetPlayerInputActions()
    {
        _aimAction = _playerInput.actions["Aim"];
        _catchAction = _playerInput.actions["Catch"];
        _moveAction = _playerInput.actions["Move"];
        _dodgeAction = _playerInput.actions["Dodge"];
    }

    public void EnableInputActions()
    {
        _aimAction.started += StartAiming;
        _aimAction.canceled += StopAiming;
        _catchAction.performed += StartCatching;
        _catchAction.canceled += StopCatching;
        _dodgeAction.performed += Dodge;
    }

    public void DisableInputActions()
    {
        _aimAction.started -= StartAiming;
        _aimAction.canceled -= StopAiming;
        _catchAction.performed -= StartCatching;
        _catchAction.canceled -= StopCatching;
        _dodgeAction.performed -= Dodge;
    }
}
