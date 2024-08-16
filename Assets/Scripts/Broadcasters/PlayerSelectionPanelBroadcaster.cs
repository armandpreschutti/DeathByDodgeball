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
    public static Action<int> onJoinSelection;
    public Action onButtonClicked;
    public Action onUpdateButtons;
    public Button panelButton;
    public GameObject panel;
    public MultiplayerEventSystem eventSystem;
    public PlayerConfigurationController playerConfigurationController;
    public bool isFilled;

    private void Awake()
    {
        panelButton.gameObject.name = $"P{playerConfigurationController.playerId}PanelButton{slotId}";
        playerId = playerConfigurationController.playerId;
        onJoinSelection?.Invoke(slotId);
    }
    private void OnEnable()
    {
        PlayerSelectionPanelObserver.onUpdateButton += UpdateButtonAvailibility;
        PlayerSelectionPanelObserver.onEnableButton += EnableButton;
        PlayerSelectionPanelObserver.onDisableButton += DisableButton;
        PlayerConfigurationController.onSubmit += SetPanelSelection;
        PlayerConfigurationController.onAddAi += SetAIPanelState;
        PlayerConfigurationController.onSubmitAi += SetAIPanelSelection;
        PlayerSelectionManager.onSetMatchSlot += SetPanelSelection;
        PlayerSelectionManager.onRemoveMatchSlot += ResetPanelSelection;
    }

    private void OnDisable()
    {
        PlayerSelectionPanelObserver.onUpdateButton -= UpdateButtonAvailibility;
        PlayerSelectionPanelObserver.onEnableButton -= EnableButton;
        PlayerSelectionPanelObserver.onDisableButton -= DisableButton;
        PlayerConfigurationController.onSubmit -= SetPanelSelection;
        PlayerConfigurationController.onAddAi -= SetAIPanelState;
        PlayerConfigurationController.onSubmitAi -= SetAIPanelSelection;
        PlayerSelectionManager.onSetMatchSlot -= SetPanelSelection;
        PlayerSelectionManager.onRemoveMatchSlot -= ResetPanelSelection;
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
        if(!isFilled)
        {
            playerConfigurationController.Submit();
        }
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

    public void SetPanelState(int pId, int currentSlot, int currentSkin)
    {
        if(currentSlot == slotId && pId ==  playerId)
        {
            if (!playerConfigurationController.isPlayerSelected && !isFilled)
            {
                panel.SetActive(true);
            }
            else
            {
                panel.SetActive(false);
            }
        }
    }
    public void SetAIPanelState(int playerId, int currentSlot, int currentSkin, int selectedSlot)
    {
        if(currentSlot == slotId && selectedSlot != slotId && eventSystem.currentSelectedGameObject == panelButton.gameObject && !isFilled)
        {
            panel.SetActive(true);
        }
    }

    public void SetAIPanelSelection(int pId, int currentSlot, int currentSkin)
    {
       
        if(currentSlot == slotId && !isFilled)
        {
            panel.SetActive(false);
            DisableButton(slotId);
        }
    }
    public void SetPanelSelection(int pId, int currentSlot, int currentSkin)
    {

        if (currentSlot == slotId)
        {
            panel.SetActive(false);
            DisableButton(slotId);
            isFilled = true;
        }
    }
    public void UpdateButtonAvailibility(int id, bool availibility)
    {
        if (id == slotId)
        {
            isFilled = availibility;
        }
    }

    public void ResetPanelSelection(int pId, int currentSlot, int currentSkin)
    {
        if(currentSlot == slotId)
        {
            if (playerConfigurationController.canSelectAI || !playerConfigurationController.isPlayerSelected)
            {
                panel.SetActive(true);
            }

        }
        
    }
}
