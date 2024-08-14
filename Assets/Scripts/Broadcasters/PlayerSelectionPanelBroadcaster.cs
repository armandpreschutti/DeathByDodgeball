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
    public static Action<int> onSelected;
    public static Action<int> onDeselected;
    public Action onUpdateButtons;
    public Button panelButton;
    public MultiplayerEventSystem eventSystem;
    public GameObject[] availibleButtons;
    public PlayerConfigurationController playerConfigurationController;

    private void Awake()
    {
        panelButton.gameObject.name = $"P{playerConfigurationController.playerID}PanelButton{slotId}";
    }
    private void OnEnable()
    {
        PlayerSelectionPanelObserver.onEnableButton += EnableButton;
        PlayerSelectionPanelObserver.onDisableButton += DisableButton;
    }

    private void OnDisable()
    {
        PlayerSelectionPanelObserver.onEnableButton -= EnableButton;
        PlayerSelectionPanelObserver.onDisableButton -= DisableButton;
    }

    public void SelectSlot()
    {
        onSelected?.Invoke(slotId);
    }

    public void DeselectSlot()
    {
        onDeselected?.Invoke(slotId);
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
}
