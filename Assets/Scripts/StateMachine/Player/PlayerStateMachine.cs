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
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Animator _baseAnim;
    [SerializeField] Animator _skinAnim;

    [Header("General")]
    [SerializeField] GameObject _equippedBall = null;
    [SerializeField] Transform _holdPosition;
    [SerializeField] Transform _holdRightPosition;
    [SerializeField] Transform _holdLeftPosition;
    [SerializeField] bool _isFacingRight;
    [SerializeField] bool _canRespawn;
    [SerializeField] float _respawnDelay;
    

    [Header("Melee")]
    [SerializeField] Vector3 _aimDirection;
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

    public Action<bool> OnDeath;
    public Action<bool> OnMove;
    public Action<bool> OnDodge;
    public Action<bool> OnHurt;
    public Action<bool> OnAim;
    public Action<bool> OnThrow;
    public Action<bool> OnCatch;
    public Action OnRespawn;

    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    public Collider2D Col { get { return _col;} set { _col = value; } }
    public SpriteRenderer SpriteRenderer { get { return _spriteRenderer;} set { _spriteRenderer = value; } }
    public Animator BaseAnim { get { return _baseAnim;} set { _baseAnim = value; } }
    public Animator SkinAnim { get { return _skinAnim; } set { _skinAnim = value; } }
    public Rigidbody2D Rb { get { return _rb; } set { _rb = value; } }  

    public GameObject EquippedBall { get {return _equippedBall;} set { _equippedBall = value; } }
    public Transform HoldPosition { get { return _holdPosition; } set { _holdPosition = value; } }
    public Transform HoldRightPosition { get { return _holdRightPosition; } set { _holdRightPosition = value; } }
    public Transform HoldLeftPosition { get { return _holdLeftPosition; } set { _holdLeftPosition = value; } }
    
    public bool CanRespawn { get { return _canRespawn; } set { _canRespawn = value; } }
    public float RespawnDelay { get { return _respawnDelay; } set { _respawnDelay = value; } }

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
        _states = new PlayerStateFactory(this);
        _currentState = _states.Active();
        _currentState.EnterState();
    }

    void Start()
    {
        SetPlayerInitialVariables();
        SetPlayerOrientation();
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
    }

    private void FixedUpdate()
    {
        _currentState.FixedUpdateStates();
    }

    public void EquipBall(GameObject ball)
    {
        if(_equippedBall == null)
        {
            ball.transform.parent = transform;
            ball.transform.position = _holdRightPosition.position;
            ball.GetComponent<BallManager>().Trajectory = Vector2.zero;
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
                _isFacingRight = transform.position.x > 0 ? true : false;
                _spriteRenderer.flipX = _isFacingRight;
                _holdPosition = _isFacingRight ? _holdRightPosition : _holdLeftPosition;
            }
            else
            {
                _isFacingRight = _moveInput.x > 0 ? true : false;
                _spriteRenderer.flipX = !_isFacingRight;
                _holdPosition = _isFacingRight ? _holdLeftPosition : _holdRightPosition;
            }
        }
        else
        {
            _isFacingRight = transform.position.x > 0 ? true : false;
            _spriteRenderer.flipX = _isFacingRight;
            _holdPosition = _isFacingRight ? _holdRightPosition : _holdLeftPosition;
        }
    }

    public void ThrowBall()
    {
        _equippedBall.GetComponent<BallManager>().SetBallActiveState(true);
        //Ctx.EquippedBall.GetComponent<Rigidbody2D>().AddForce(Ctx.AimDirection * Ctx.CurrentThrowPower, ForceMode2D.Impulse
        _equippedBall.GetComponent<BallManager>().Trajectory = _aimDirection * _currentThrowPower * Time.deltaTime;
        UnequipBall(_equippedBall);
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
