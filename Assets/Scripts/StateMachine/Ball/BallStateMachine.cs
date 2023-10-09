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
    public BallSelfDestructState SelfDestructState = new BallSelfDestructState();

    [Header("Components")]
    [SerializeField] Collider2D _col;
    [SerializeField] Rigidbody2D _rb;

    [Header("Variables")]
    [SerializeField] Transform _parent;
    [SerializeField] float _ballDamage;
    [SerializeField] float _collsionReactivationTime;    
    
    public Collider2D Col { get { return _col; } set { _col = value; } }
    public Rigidbody2D Rb { get { return _rb; } set { _rb = value; } }

    public Transform Parent { get { return _parent; } set { _parent = value; } }   
    public float BallDamage { get { return _ballDamage; } set { _ballDamage = value; } }
    public float CollsionReactivationTime { get { return _collsionReactivationTime; } set { _collsionReactivationTime = value; } }

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

    public void EnterSelfDestructState()
    {
        _currentState = SelfDestructState;
        _currentState.EnterState(this);
    }

    public void SetBallComponents()
    {
        _col = GetComponent<Collider2D>();  
        _rb = GetComponent<Rigidbody2D>();
    }
}
