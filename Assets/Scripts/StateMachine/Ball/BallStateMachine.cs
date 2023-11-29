using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class BallStateMachine : MonoBehaviour
{
    public BallBaseState _currentState;
    public BallActiveState ActiveState = new BallActiveState();
    public BallEquippedState EquippedState = new BallEquippedState();
    public BallUnequippedState UnequippedState = new BallUnequippedState();

    [Header("Components")]
    [SerializeField] Collider2D _col;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] SpriteRenderer _spriteRenderer;

    [Header("VFX")]
    [SerializeField] ParticleSystem _normalBallVFX;
    [SerializeField] ParticleSystem _superBallVFX;
    [SerializeField] ParticleSystem _ballExplosionVFX;
    [SerializeField] GameObject _explosionImpact;
    [SerializeField] Sprite _ballSprite;
    [SerializeField] Sprite _bombSprite;

    [Header("Variables")]
    [SerializeField] Transform _parent;
    [SerializeField] float _ballDamage;
    [SerializeField] float _knockBackPower;
    
    public Collider2D Col { get { return _col; } set { _col = value; } }
    public Rigidbody2D Rb { get { return _rb; } set { _rb = value; } }
    public SpriteRenderer SpriteRenderer { get { return _spriteRenderer; } set { _spriteRenderer = value; } }
    public ParticleSystem NormalBallVFX {get { return _normalBallVFX; } set { _normalBallVFX = value; } } 
    public ParticleSystem SuperBallVFX { get { return _superBallVFX; } set { _superBallVFX = value; } }
    public ParticleSystem BallExplosionVFX { get { return _ballExplosionVFX; } set { _ballExplosionVFX = value; } }
    public Transform Parent { get { return _parent; } set { _parent = value; } }   
    public float BallDamage { get { return _ballDamage; } set { _ballDamage = value; } }
    public float KnockBackPower { get { return _knockBackPower; } set { _knockBackPower= value; } }

    public static event Action OnExplosion;

    private void Awake()
    {
        SetBallComponents();
        _currentState = UnequippedState;
        _currentState.EnterState(this);
    }

    void Start()
    {
        if(GameObject.Find("CenterBound") != null)
        {
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), GameObject.Find("CenterBound").GetComponent<Collider2D>());
        }        
    }

    void Update()
    {
        _currentState.UpdateState(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _currentState.OnTriggerEnter2D(this, collision);
    }

    public void SwitchState(BallBaseState newState)
    {
        _currentState = newState;
        newState.EnterState(this);
    }

    public void EnterUnequippedState()
    {
        _currentState = UnequippedState;
        _currentState.EnterState(this);
    }

    public void EnterEquippedState()
    {
        _currentState = EquippedState;
        _currentState.EnterState(this);
    }

    public void EnterActiveState(float ballDamage)
    {
        _ballDamage = ballDamage;
        _currentState = ActiveState;
        _currentState.EnterState(this);
    }

    public void SetBallComponents()
    {
        _col = GetComponent<Collider2D>();  
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void InstantiateBallExplosion()
    {
        OnExplosion?.Invoke();
        Instantiate(_ballExplosionVFX, transform.position, Quaternion.identity);
        Instantiate(_explosionImpact, transform.position, Quaternion.identity);
    }
    public void SetSprite(float value)
    {
        if(value >= 35f)
        {
            SpriteRenderer.sprite = _bombSprite;
        }
        else
        {
            SpriteRenderer.sprite = _ballSprite;
        }
    }
}
