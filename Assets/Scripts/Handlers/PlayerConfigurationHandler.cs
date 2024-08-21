using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using TMPro;
using UnityEngine.UI;

public class PlayerConfigurationHandler_Depricated : MonoBehaviour
{
    [Header("General")]
    [SerializeField] int _playerId;
    [SerializeField] bool _playerReady = false;
    [SerializeField] PlayerManager_Depricated _playerManager;
    [SerializeField] PlayerCostumizationSO _playerCostumizationSO;
    [SerializeField] Image _previewImage;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI _playerTeamPrompt;
    [SerializeField] TextMeshProUGUI _playerReadyPrompt;
    [SerializeField] GameObject _configurationCover;
    

    [Header("Input")]
    [SerializeField] PlayerInput _playerInput;
    [SerializeField] InputAction _switchTeamAction;
    [SerializeField] InputAction _playerReadyAction;
    [SerializeField] InputAction _exitToMainAction;
    [SerializeField] InputAction _switchSkinAction;

    public bool PlayerReady { get { return _playerReady; } }
    public PlayerManager_Depricated PlayerManager { get { return _playerManager; } }

    public static event Action<PlayerConfigurationHandler_Depricated, bool> onPlayerReady;
    public static event Action<PlayerConfigurationHandler_Depricated> onPlayerToggleTeam;
    public static event Action<PlayerConfigurationHandler_Depricated> onPlayerJoinedSession;
    public static event Action onPlayerExit;


    private void OnEnable()
    {
       // GameManager.onPlayerFound += InitializePlayerConfiguration;
        PlayerReadyObserver.onPrimePlayer += PrimePlayerForMatch;
        PlayerExitObserver.onPlayerExit += PromptPlayerExit; 
    }

    private void OnDisable()
    {
       // GameManager.onPlayerFound -= InitializePlayerConfiguration;
        PlayerReadyObserver.onPrimePlayer -= PrimePlayerForMatch;
        PlayerExitObserver.onPlayerExit -= PromptPlayerExit;
        _switchTeamAction.performed -= TogglePlayerTeam;
        _playerReadyAction.performed -= TogglePlayerReady;
        _exitToMainAction.performed -= ExitToMenu;
        _switchSkinAction.performed -= TogglePlayerSkin;
    }

    public void InitializePlayerConfiguration(PlayerInput playerInput)
    {
        if(playerInput.playerIndex +1 == _playerId)
        {
            Vector3 randomVector = new Vector3(UnityEngine.Random.Range(-6f, 6f), UnityEngine.Random.Range(-2f, 2f), 0f);
            playerInput.transform.position = randomVector;
            playerInput.name = $"Player{_playerId}";
            _playerManager = playerInput.GetComponent<PlayerManager_Depricated>();
            _playerManager.ConfigurePlayerInstance(_playerId);
            UpdateUIText(_playerTeamPrompt, _playerManager.TeamId.ToString());
           // _playerCostumizationSO.GetRandomSkin(_playerManager);
            _previewImage.sprite = _playerManager.GetComponent<SpriteRenderer>().sprite;
            _configurationCover.SetActive(false);
            InitializePlayerConfigurationControl(playerInput);
            onPlayerJoinedSession?.Invoke(this);
        }
        else
        {
            return;
        }
    }

    public void TogglePlayerTeam(InputAction.CallbackContext context)
    {
        switch (_playerManager.TeamId)
        {
            case 1:
                _playerManager.TeamId = 2;
                break;
            case 2:
                _playerManager.TeamId = 1;
                break;
        }
     //   _playerCostumizationSO.ToggleTeamSkin(_playerManager);
        _previewImage.sprite = _playerManager.GetComponent<SpriteRenderer>().sprite;
        onPlayerToggleTeam?.Invoke(this);
        UpdateUIText(_playerTeamPrompt, _playerManager.TeamId.ToString());
    }

    public void TogglePlayerSkin(InputAction.CallbackContext context)
    {
      //  _playerCostumizationSO.TogglePlayerSkin(_playerManager);
        _previewImage.sprite = _playerManager.GetComponent<SpriteRenderer>().sprite;
    }

    public void InitializePlayerConfigurationControl(PlayerInput playerInput)
    {
        _playerInput = playerInput;
        _switchTeamAction = _playerInput.actions["SwitchTeam"];
        _switchSkinAction = _playerInput.actions["SwitchSkin"];
        _playerReadyAction = _playerInput.actions["PlayerReady"];
        _exitToMainAction = _playerInput.actions["BackButton"];
        _switchTeamAction.performed += TogglePlayerTeam;
        _switchSkinAction.performed += TogglePlayerSkin;
    }

    public void UpdateUIText(TextMeshProUGUI prompt, string message)
    {
        prompt.text = message ;
    }

    public void PrimePlayerForMatch(PlayerManager_Depricated playerManager, bool value)
    {
        if(playerManager == _playerManager)
        {
            if (value)
            {

                _playerReadyAction.performed += TogglePlayerReady;

            }
            else
            {
                _playerReadyAction.performed -= TogglePlayerReady;
                return;
            }
        }
        else
        {
            return;
        }
       
    }
    public void TogglePlayerReady(InputAction.CallbackContext context)
    {
        _playerReady = !_playerReady;
        onPlayerReady?.Invoke(this, _playerReady);
        if(_playerReady)
        {
            _switchTeamAction.performed -= TogglePlayerTeam;
        }
        else
        {
            _switchTeamAction.performed += TogglePlayerTeam;
        }
    }
    public void PromptPlayerExit(PlayerManager_Depricated playerManager, bool value)
    {
        if (value)
        {
            _exitToMainAction.performed += ExitToMenu;
        }
        else
        {
            _exitToMainAction.performed -= ExitToMenu;
        }
    }
    public void ExitToMenu(InputAction.CallbackContext context)
    {
        onPlayerExit?.Invoke();
    }
}
