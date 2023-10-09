using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] bool _debugMode;
    [SerializeField] bool _isReady;

    [Header("Components")]
    public PlayerInput _playerInput;
    public PlayerStateMachine _playerStateMachine;

    [Header("Input")]
    [SerializeField] InputAction _readyAction;

    public bool IsReady { get { return _isReady; } set { _isReady = value; } }
    public bool DebugMode { get { return _debugMode; } set { _debugMode= value; } }

    private void Awake()
    {
        SetPlayerComponents();
    }
    private void OnEnable()
    {
        _readyAction.performed += Ready;
    }
    private void OnDisable()
    {
        _readyAction.performed -= Ready;
    }

    public void Start()
    {
        InitializePlayer();
    }

    public void InitializePlayer()
    {
        if (!_debugMode)
        {
            if (_playerInput != null)
            {
                GameManager.GetInstance().AddPlayer(this);
                name = $"Player{_playerInput.playerIndex + 1}";
                DontDestroyOnLoad(gameObject);
                GameObject.Find($"{name}Prompt").GetComponent<TextMeshProUGUI>().text = "Press LB or F when ready";
            }
        }
        else
        {
            return;
        }
        
    }

    public void Ready(InputAction.CallbackContext context)
    {
        if (!_debugMode)
        {
            if (!_isReady)
            {
                _isReady = true;
                GameManager.GetInstance().PlayerReady(this);
                GameObject.Find($"{name}Prompt").GetComponent<TextMeshProUGUI>().text = "Ready!";
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
        
    }

    public void ActivatePlayer()
    {
        _playerStateMachine.enabled = true;
    }

    public void SetPlayerComponents()
    {
        DontDestroyOnLoad(gameObject);
        _playerInput = GetComponent<PlayerInput>();
        _playerStateMachine = GetComponent<PlayerStateMachine>();
        _readyAction = _playerInput.actions["Ready"];
    }
}
