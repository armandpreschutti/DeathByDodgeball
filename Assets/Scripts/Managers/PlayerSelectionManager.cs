using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class PlayerSelectionManager : MonoBehaviour
{
    public PlayerConfigurationSO[] playerConfigurations;
    public static Action<int, int, int> onSetMatchSlot;
    public static Action<int, int, int> onRemoveMatchSlot;
    public static Action<int, bool> onResetPlayer;
    public bool isMatchReady;
    private void Awake()
    {
        playerConfigurations = new PlayerConfigurationSO[4];
    }

    private void OnEnable()
    {
        PlayerConfigurationController.onSubmit += AddPlayerToMatchConfiguration;
        PlayerConfigurationController.onSubmitAi += AddPlayerToMatchConfiguration;
        PlayerConfigurationController.onRemoveSelection += RemovePlayerFromMatchConfiguration;
        
    }

    private void OnDisable()
    {
        PlayerConfigurationController.onSubmit -= AddPlayerToMatchConfiguration;
        PlayerConfigurationController.onSubmitAi -= AddPlayerToMatchConfiguration;
        PlayerConfigurationController.onRemoveSelection -= RemovePlayerFromMatchConfiguration;
    }

    public void AddPlayerToMatchConfiguration(int playerId, int slotId, int skinId)
    {
        // Create a new PlayerConfigurationSO object
        PlayerConfigurationSO newPlayerConfig = ScriptableObject.CreateInstance<PlayerConfigurationSO>();

        // Assign values to the new object (replace with actual properties)
        newPlayerConfig.playerId = playerId;
        newPlayerConfig.skinID = skinId;

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
        if (playerConfigurations[slotId -1] != null && (playerConfigurations[slotId -1].playerId == playerId || playerConfigurations[slotId-1].playerId == 0))
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
                playerConfigurations[i].SetTeam(i + 1);
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
            isMatchReady = true;
        }
        else
        {
            isMatchReady = false;
        }
    }
    
}
