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
    }
    public void RemovePlayerFromMatchConfiguration(int playerId, int slotId, int skinId)
    {
        /*// Create a new PlayerConfigurationSO object
        PlayerConfigurationSO newPlayerConfig = ScriptableObject.CreateInstance<PlayerConfigurationSO>();

        // Assign values to the new object (replace with actual properties)
        newPlayerConfig.playerId = playerId;
        newPlayerConfig.skinID = skinId;

        // Ensure the array has enough space
        if (playerConfigurations.Length <= slotId)
        {
            Array.Resize(ref playerConfigurations, slotId);
        }

        // Assign the new object to the specified slot
        playerConfigurations[slotId - 1] = newPlayerConfig;
        onSetMatchSlot?.Invoke(playerId, slotId, skinId);*/
        Debug.Log($"P{playerId}trying to remove Slot{slotId}");
    }
}
