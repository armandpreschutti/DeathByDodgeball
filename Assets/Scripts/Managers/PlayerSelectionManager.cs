using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PlayerSelectionManager : MonoBehaviour
{
    public PlayerConfigurationSO[] playerConfigurations;
    public PlayerInputManager playerInputManager;
    public static Action<int, int, int> onSetMatchSlot;
    public static Action<int, int, int> onRemoveMatchSlot;
    public static Action<int, bool> onResetPlayer;
    public static Action onMatchReady;
    public static Action onMatchInitiated;
    public static Action onMatchAbort;
    public static Action onMatchStart;
    public bool isMatchReady;
    public bool isMatchStarting;
    public PlayableDirector playableDirector;
    public GameObject matchStartPrompt;
    public GameObject matchInitiatedPrompt;

    private void Awake()
    {
        playerConfigurations = new PlayerConfigurationSO[4];
        playableDirector= GetComponent<PlayableDirector>();
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    private void OnEnable()
    {
        PlayerConfigurationController.onSubmit += AddPlayerToMatchConfiguration;
        PlayerConfigurationController.onSubmitAi += AddPlayerToMatchConfiguration;
        PlayerConfigurationController.onRemoveSelection += RemovePlayerFromMatchConfiguration;
        PlayerConfigurationController.onInitiateMatchStart += InitiateMatch;
        PlayerConfigurationController.onRemoveSelection += AbortMatch;
    }

    private void OnDisable()
    {
        PlayerConfigurationController.onSubmit -= AddPlayerToMatchConfiguration;
        PlayerConfigurationController.onSubmitAi -= AddPlayerToMatchConfiguration;
        PlayerConfigurationController.onRemoveSelection -= RemovePlayerFromMatchConfiguration;
        PlayerConfigurationController.onInitiateMatchStart -= InitiateMatch;
        PlayerConfigurationController.onRemoveSelection -= AbortMatch;
    }

    public void AddPlayerToMatchConfiguration(int playerId, int slotId, int skinId)
    {
        // Create a new PlayerConfigurationSO object
        PlayerConfigurationSO newPlayerConfig = ScriptableObject.CreateInstance<PlayerConfigurationSO>();

        // Assign values to the new object (replace with actual properties)
        newPlayerConfig.playerId = playerId;
        newPlayerConfig.skinID = skinId;
        newPlayerConfig.slotId = slotId;

        // Ensure the array has enough space
        if (playerConfigurations.Length <= slotId)
        {
            Array.Resize(ref playerConfigurations, slotId );
        }

        // Assign the new object to the specified slot
        playerConfigurations[slotId - 1] = newPlayerConfig;
        onSetMatchSlot?.Invoke(playerId, slotId, skinId);
        CheckMatchReadyState();

    }
    public void RemovePlayerFromMatchConfiguration(int playerId, int slotId, int skinId)
    {
        if (playerConfigurations[slotId -1] != null && (playerConfigurations[slotId -1].playerId == playerId || playerConfigurations[slotId-1].playerId == 0) && !isMatchStarting)
        {
            if (playerConfigurations[slotId - 1] != null && (playerConfigurations[slotId - 1].playerId == playerId || playerConfigurations[slotId - 1].playerId == 0))
            {
                if (playerConfigurations[slotId - 1].playerId == playerId)
                {
                    onResetPlayer?.Invoke(playerId, false);
                }
                playerConfigurations[slotId - 1] = null;
                onRemoveMatchSlot?.Invoke(playerId, slotId, skinId);
                CheckMatchReadyState();
            }
        }
    }

    public void CheckMatchReadyState()
    {
        int readyBluePlayers = 0;
        int readyRedPlayers= 0;
        for (int i = 0; i < playerConfigurations.Length; i++)
        {

            if (playerConfigurations[i]!= null)
            {
                playerConfigurations[i].SetTeam();
                if (playerConfigurations[i].teamId == 1)
                {
                    readyBluePlayers++;
                }
                else if (playerConfigurations[i].teamId == 2)
                {
                    readyRedPlayers++;
                }
            }
        }
        if(readyBluePlayers >= 1 && readyRedPlayers >= 1)
        {
            matchStartPrompt.SetActive(true);
            isMatchReady = true;
            onMatchReady?.Invoke();

        }
        else
        {
            matchStartPrompt.SetActive(false);
            isMatchReady = false;

        }
    }


    public void InitiateMatch()
    {
        if (isMatchReady)
        {
            isMatchStarting = true;
            matchStartPrompt.SetActive(false);
            matchInitiatedPrompt.SetActive(true);
            playableDirector.Play();
            onMatchInitiated?.Invoke();
        }
        else
        {
            Debug.LogError("Not enough players on both teams");
            matchStartPrompt.SetActive(false);
        }
    }

    public void AbortMatch(int playerId, int currentSlot, int currentSkin)
    {
        if(isMatchStarting) 
        {
            isMatchStarting = false;
            matchStartPrompt.SetActive(true);
            matchInitiatedPrompt.SetActive(false);
            playableDirector.initialTime = 0.0f;
            playableDirector.Stop();
            onMatchAbort?.Invoke();
        }

    }

    public void StartMatch()
    {
       

    }
}
