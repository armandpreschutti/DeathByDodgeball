using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] int _playerId;
    [SerializeField] int _teamId;
    [SerializeField] bool _isInvicible;
    [SerializeField] Color _teamColor;

    [Header("Components")]
    [SerializeField] PlayerInput _playerInput;
    [SerializeField] PlayerStateMachine _playerStateMachine;
    [SerializeField] HealthHandler _healthHandler;
    [SerializeField] AudioSource _playerAudio;
    public GameObject readyPrompt;
    public GameObject exitPrompt;

    public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }
    public HealthHandler HealthHandler { get { return _healthHandler; } }  
    public AudioSource PlayerAudio { get { return _playerAudio; } }
    public int PlayerId { get { return _playerId;} set { _playerId = value; } }
    public int TeamId { get { return _teamId; } set { _teamId = value; } }
    public bool IsInvicible { get { return _isInvicible; } set {  _isInvicible = value; } }
    public Color TeamColor { get { return _teamColor; } set { _teamColor = value; } }

    public static event Action<PlayerManager> OnPlayerDeath;

    
    private void OnEnable()
    {
        LocalMatchManager.onActivatePlayers += ActivatePlayer;
        LocalMatchManager.onDeactivatePlayers+= DeactivatePlayer;
        LocalMatchManager.onResetPlayers += ResetPlayer;
    }
    private void OnDisable()
    {
        LocalMatchManager.onActivatePlayers -= ActivatePlayer;
        LocalMatchManager.onDeactivatePlayers -= DeactivatePlayer;
        LocalMatchManager.onResetPlayers -= ResetPlayer;
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SetPlayerComponents();
    }

    public void ActivatePlayer()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        TogglePlayerInvicibility(false);
        _playerStateMachine.IsMobile = true;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    public void DeactivatePlayer()
    {
        TogglePlayerInvicibility(true);
        _playerStateMachine.IsMobile = false;
        _playerStateMachine.DestroyBall();
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    public void ResetPlayer()
    {
        _playerInput.SwitchCurrentActionMap("Player");
        _healthHandler.ResetLives();
        _playerStateMachine.IsDead = false;
        _playerStateMachine.DestroyBall();
    }
  
    public void TogglePlayerInvicibility(bool value)
    {
        if (_playerStateMachine != null)
        {
            _playerStateMachine.IsInvicible = value;
        }
        if (_healthHandler != null)
        {
            _healthHandler.IsInvicible = value;
        }

    }
    public void SetPlayerComponents()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerStateMachine = GetComponent<PlayerStateMachine>();
        _healthHandler = GetComponent<HealthHandler>();
        _playerAudio = GetComponent<AudioSource>();
    }

    public void ConfigurePlayerInstance(int playerId)
    {
        _playerId= playerId;
        switch (_playerId)
        {
            case 1:
                _teamId = 1;
                break;
            case 2:
                _teamId = 2;
                break;
            case 3:
                _teamId = 1;
                break;
            case 4:
                _teamId = 2;
                break;
            default:
                _teamId = 1;
                break;
        }
    }

    public void Die()
    {
        OnPlayerDeath?.Invoke(this);
    }
    public void HidePlayer(bool value)
    {
        GetComponent<SpriteRenderer>().enabled = !value;
        if(value)
        {
            DeactivatePlayer();
        }
        else
        {
            ActivatePlayer();
        }
    }
}
