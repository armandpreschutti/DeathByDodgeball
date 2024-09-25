using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUBrain : MonoBehaviour
{
    [Header("States")]
    [SerializeField] CPUBaseState _currentState;
    [SerializeField] CPUStateFactory _states;
    public string CurrentSuperState;
    public string CurrentSubState;

    public int playerId;
    public PawnManager pawnManager;
    public PlayerStateMachine playerStateMachine;
    public bool isActivated;
    public bool isFacingLeft;

    public int moveX;
    public int moveY;
    public Vector2 moveInput;
    public GameObject closestFreeBall;

    public CPUBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }


    private void Awake()
    {
        _states = new CPUStateFactory(this);
        _currentState = _states.Active();
        _currentState.EnterState();
        transform.Find("CPUDetection").gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        MatchInstanceManager.onInitializeMatchInstance += SetStateMachine;
        MatchInstanceManager.onEnablePawnControl += EnableControl;
        MatchInstanceManager.onDisablePawnControl += DisableControl;
    }

    private void OnDisable()
    {
        MatchInstanceManager.onInitializeMatchInstance -= SetStateMachine;
        MatchInstanceManager.onEnablePawnControl -= EnableControl;
        MatchInstanceManager.onDisablePawnControl -= DisableControl;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.UpdateStates();
        if (isActivated)
        {
            SetMoveInput();
            SetCPUOrientation();
        }


    }

    private void FixedUpdate()
    {
        _currentState.FixedUpdateStates();
    }


    public void SetStateMachine()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
        pawnManager = GetComponent<PawnManager>();
        playerId = pawnManager.playerId;
    }
    public void EnableControl()
    {
        isActivated = true;
        playerStateMachine.MoveInput = Vector2.zero;
    }
    public void DisableControl()
    {
        isActivated = false;
        playerStateMachine.MoveInput = Vector2.zero;
    }
    public void SetMoveInput()
    {
        moveInput = new Vector2(moveX, moveY).normalized;
        playerStateMachine.MoveInput = moveInput;
    }

    public void SetCPUOrientation()
    {
        isFacingLeft = transform.position.x > 0 ? true : false;
    }
}
