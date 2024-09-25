using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.Playables;
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
    public bool isLockedIn;


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
        PlayerSelectionManager.onMatchInitiated += LockSelection;
        PlayerSelectionManager.onMatchAbort += UnlockSelection;
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
        PlayerSelectionManager.onMatchInitiated -= LockSelection;
        PlayerSelectionManager.onMatchAbort -= UnlockSelection;

    }

    public void SelectSlot()
    {
        onSelected?.Invoke(slotId);
        SetPanelState(playerConfigurationController.playerId, slotId, playerConfigurationController.currentSkin);
    }

    public void DeselectSlot()
    {
        onDeselected?.Invoke(slotId);
        playerConfigurationController.canSelectAI = false;
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
            if (!playerConfigurationController.isPlayerSelected && !isFilled && !isLockedIn)
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
        if(currentSlot == slotId && selectedSlot != slotId && eventSystem.currentSelectedGameObject == panelButton.gameObject && !isFilled && !isLockedIn)
        {
            panel.SetActive(true);
        }
    }

    public void SetAIPanelSelection(int pId, int currentSlot, int currentSkin, Color color)
    {
       
        if(currentSlot == slotId && !isFilled && !isLockedIn)
        {
            panel.SetActive(false);
            DisableButton(slotId);
        }
    }
    public void SetPanelSelection(int pId, int currentSlot, int currentSkin, Color color)
    {

        if (currentSlot == slotId && !isLockedIn)
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

    public void ResetPanelSelection(int pId, int currentSlot, int currentSkin, Color color)
    {
        if(currentSlot == slotId)
        {
            if (playerConfigurationController.canSelectAI || !playerConfigurationController.isPlayerSelected)
            {
                panel.SetActive(true);
            }

        }        
    }

    public void LockSelection()
    {
        isLockedIn = true;
    }

    public void UnlockSelection()
    {
        isLockedIn = false;
    }
}
