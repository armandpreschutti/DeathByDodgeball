using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PreMatchManager : MonoBehaviour
{
    [SerializeField] List<GameObject> _currentPlayers = new List<GameObject>();
    [SerializeField] List<GameObject> _readyPlayers = new List<GameObject>();
    public List<GameObject> _team1 = new List<GameObject>();
    public List<GameObject> _team2 = new List<GameObject>();

    private void OnEnable()
    {
        PlayerConfigurationHandler.onPlayerJoinedSession += AddPlayerToMatch;
        PlayerConfigurationHandler.onPlayerReady += AddToReadyPlayers;
        PlayerConfigurationHandler.onPlayerReady += SetPlayerTeam;
        PlayerConfigurationHandler.onPlayerReady += CheckStartGame;
        PlayerConfigurationHandler.onPlayerExit += ExitMatchConfiguration;
    }

    private void OnDisable()
    {
        PlayerConfigurationHandler.onPlayerJoinedSession -= AddPlayerToMatch;
        PlayerConfigurationHandler.onPlayerReady -= AddToReadyPlayers;
        PlayerConfigurationHandler.onPlayerReady -= SetPlayerTeam;
        PlayerConfigurationHandler.onPlayerReady -= CheckStartGame;
        PlayerConfigurationHandler.onPlayerExit -= ExitMatchConfiguration;

    }
    private void Start()
    {
        GameManager.GetInstance().EnableJoining();
    }

    private void AddPlayerToMatch(PlayerConfigurationHandler playerConfig)
    {
        _currentPlayers.Add(playerConfig.PlayerManager.gameObject);
        playerConfig.PlayerManager.TogglePlayerInvicibility(true);
    }

    private void AddToReadyPlayers(PlayerConfigurationHandler playerConfig, bool value)
    {
        if(value)
        {
            _readyPlayers.Add(playerConfig.PlayerManager.gameObject);
            playerConfig.PlayerManager.HidePlayer(true);
            playerConfig.PlayerManager.readyPrompt.SetActive(false);
        }
        else
        {
             _readyPlayers.Remove(playerConfig.PlayerManager.gameObject);
            playerConfig.PlayerManager.HidePlayer(false);
            playerConfig.PlayerManager.TogglePlayerInvicibility(true);
        }

    }

    private void SetPlayerTeam(PlayerConfigurationHandler playerConfig, bool value)
    {
        if (playerConfig.PlayerManager.TeamId == 1 )
        {
            if (value)
            {
                _team1.Add(playerConfig.PlayerManager.gameObject);
            }
            else
            {
                _team1.Remove(playerConfig.PlayerManager.gameObject);
            }

        }
        else if (playerConfig.PlayerManager.TeamId == 2)
        {
            if (value)
            {
                _team2.Add(playerConfig.PlayerManager.gameObject);
            }
            else
            {
                _team2.Remove(playerConfig.PlayerManager.gameObject);
            }
        }
        else
        {
            Debug.LogError($"Problem setting {playerConfig.PlayerManager.name}");
            return;
        }
    }
    public void CheckStartGame(PlayerConfigurationHandler playerConfig, bool value)
    {
        if(_team1.Count != 0 && _team2.Count != 0)
        {
            if(_readyPlayers.Count == _currentPlayers.Count)
            {
                Debug.LogWarning("Game is ready to start");
                StartCoroutine(StartGame());
            }
            else
            {
                StopAllCoroutines();
            }
        }
        else
        {
            return;
        }
    }
    IEnumerator StartGame()
    {
        /*        Debug.LogWarning("Game starting in ...");
                yield return new WaitForSeconds(1);
                Debug.LogWarning("3");
                yield return new WaitForSeconds(1);
                Debug.LogWarning("2");
                yield return new WaitForSeconds(1);
                Debug.LogWarning("1");
                yield return new WaitForSeconds(1);*/
        yield return new WaitForSeconds(1);
        CreateLocalMatchInstance();
        GameManager.GetInstance().SwitchScene("Gameplay");
    }
    public void CreateLocalMatchInstance()
    {
        LocalMatchManager localMatchManager = this.gameObject.AddComponent<LocalMatchManager>();
        localMatchManager.currentPlayers = _currentPlayers;
        localMatchManager.team1 = _team1;
        localMatchManager.team2 = _team2;
        this.enabled = false;
    }
    IEnumerator ResetConfigurationSettings()
    {
        foreach (GameObject player in _currentPlayers)
        {
            player.GetComponent<PlayerManager>().DeactivatePlayer();
        }
        yield return new WaitForSeconds(1.5f);
        foreach (GameObject player in _currentPlayers)
        {
            Destroy(player.gameObject);
        }
        _currentPlayers.Clear();
        _readyPlayers.Clear();
        _team1.Clear();
        _team2.Clear();
        GameManager.GetInstance().DisableJoining();
        Destroy(this);
    }

    public void ExitMatchConfiguration()
    {
        StartCoroutine(ResetConfigurationSettings());
        GameManager.GetInstance().SwitchScene("MainMenu");

    }

}
