using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class PlayerSelectionPanelBroadcaster : MonoBehaviour
{
    public int slotId;
    public int playerId;
    public static Action<int> onSelected;
    public static Action<int> onDeselected;
    public Action onButtonClicked;
    public Action onUpdateButtons;
    public Button panelButton;
    public GameObject panel;
    public MultiplayerEventSystem eventSystem;
    public PlayerConfigurationController playerConfigurationController;

    private void Awake()
    {
        panelButton.gameObject.name = $"P{playerConfigurationController.playerId}PanelButton{slotId}";
        playerId = playerConfigurationController.playerId;
    }
    private void OnEnable()
    {
        PlayerSelectionPanelObserver.onEnableButton += EnableButton;
        PlayerSelectionPanelObserver.onDisableButton += DisableButton;
        PlayerConfigurationController.onSubmit += SetPanelState;
    }

    private void OnDisable()
    {
        PlayerSelectionPanelObserver.onEnableButton -= EnableButton;
        PlayerSelectionPanelObserver.onDisableButton -= DisableButton;
        PlayerConfigurationController.onSubmit -= SetPanelState;
    }

    public void SelectSlot()
    {
        onSelected?.Invoke(slotId);
        SetPanelState(playerConfigurationController.playerId, slotId, playerConfigurationController.currentSkin);
    }

    public void DeselectSlot()
    {
        onDeselected?.Invoke(slotId);
        panel.SetActive(false);
    }

    public void SlotButtonPressed()
    {
        onButtonClicked?.Invoke();
    }

    public void EnableButton(int id)
    {
        
        if(id == slotId)
        {
            panelButton.gameObject.SetActive(true);
        }
    }
    public void DisableButton(int id)
    {
        if (id == slotId)
        {
            if (eventSystem.currentSelectedGameObject != panelButton.gameObject)
            {
                panelButton.gameObject.SetActive(false);
            }
        }
    }

    public void SetPanelState(int Id, int currentSlot, int currentSkin)
    {
        if(currentSlot == slotId && Id ==  playerId)
        {
            if (!playerConfigurationController.isPlayerSelected /*&& playerConfigurationController.eventSystem.currentSelectedGameObject == panelButton*/)
            {
                panel.SetActive(true);
            }
            else
            {
                panel.SetActive(false);
            }
        }
    }
    public void SetAISelection(int id)
    {
        if(id == slotId)
        {
            Debug.Log($"P{playerConfigurationController.playerId} wants to add an AI at slot {playerConfigurationController.currentSlot}");
        }
        
    }
}
