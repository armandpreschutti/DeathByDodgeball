using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameInstance;
    public GameObject[] currentPlayers;
    public bool isPaused;

    public static GameManager GetInstance()
    {
        return gameInstance;
    }

    private void Awake()
    {
        if (gameInstance == null)
        {
            gameInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        PlayerConfigurationController.onDestroyAllPlayers += ClearPlayersFromGame;
        PlayerManager.onJoin += AddPlayerToGame;
        PauseMenuController.OnGamePaused += PauseGame;
    }
    private void OnDisable()
    {
        PlayerConfigurationController.onDestroyAllPlayers -= ClearPlayersFromGame;
        PlayerManager.onJoin -= AddPlayerToGame;
        PauseMenuController.OnGamePaused -= PauseGame;
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

}
