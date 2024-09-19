using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gameInstance;
    public GameObject[] currentPlayers;
    public PlayerConfigurationSO[] playerConfigurations;
    public bool isPaused;
    public PlayerInputManager playerInputManager;
    public string winningTeam;
    public bool isDebugging;
    public float idleTime;
    public float restartTime; 
    public static GameManager GetInstance()
    {
        return gameInstance;
    }

    private void Awake()
    {
        if (gameInstance == null)
        {
            gameInstance = this;
            winningTeam = "Draw";
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        SetIdleTimer();
    }

    private void OnEnable()
    {
        PlayerConfigurationController.onDestroyAllPlayers += ClearPlayersFromGame;
        PlayerManager.onJoin += AddPlayerToGame;
        PauseMenuController.OnGamePaused += PauseGame;
        SceneManager.sceneLoaded += ResetCurrentPlayers;
      //  UserController.onInputTriggered += ResetIdleTime;
    }

    private void OnDisable()
    {
        PlayerConfigurationController.onDestroyAllPlayers -= ClearPlayersFromGame;
        PlayerManager.onJoin -= AddPlayerToGame;
        PauseMenuController.OnGamePaused -= PauseGame;
        SceneManager.sceneLoaded -= ResetCurrentPlayers;
       // UserController.onInputTriggered -= ResetIdleTime;
    }

    public void AddPlayerToGame(int playerId, GameObject playerObject)
    {
        // Find the first available (null) index in the currentPlayers array
        for (int i = 0; i < currentPlayers.Length; i++)
        {
            if (currentPlayers[i] == null)
            {
                currentPlayers[i] = playerObject;

                return; // Exit the function once the player is added
            }
        }
    }

    public void ClearPlayersFromGame()
    {
        for (int i = 0; i < currentPlayers.Length; i++)
        {
            Destroy(currentPlayers[i]);
            currentPlayers[i] = null;
        }
    }

    public void PauseGame(bool value)
    {
        isPaused = value;
        Time.timeScale = value ? 0.0f : 1.0f;

        PlayerStateMachine[] pausedEntities = FindObjectsOfType<PlayerStateMachine>();
        for (int i = 0; i < pausedEntities.Length; i++)
        {
            pausedEntities[i].enabled = !value;
        }
    }

    public void ResetCurrentPlayers(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "MainMenu"|| scene.name == "TitleMenu")
        {
            for(int i = 0; i < currentPlayers.Length; i++)
            {
                if (currentPlayers[i].gameObject != null)
                {
                    Destroy(currentPlayers[i].gameObject);
                }
                currentPlayers[i] = null;
            }
            for (int i = 0; i < playerConfigurations.Length; i++)
            {
                playerConfigurations[i] = null;
            }
            isPaused = false;
            Time.timeScale = 1.0f;
        }
        else if(scene.name != "MatchInstance")
        {
            Time.timeScale = 1.0f;
        }
        if (!isDebugging)
        {
            if (scene.name == "PlayerSelection")
            {
                playerInputManager.EnableJoining();
            }
            else
            {
                playerInputManager.DisableJoining();
            }
        }
    }

    public void SetIdleTimer()
    {
        if(!isDebugging)
        {
            idleTime += Time.deltaTime;
            if (idleTime >= restartTime)
            {
                SceneManager.LoadScene("TitleMenu");
                idleTime = 0f;
            }
        }
    }

    public void ResetIdleTime()
    {
        idleTime = 0.0f;
    }
}
