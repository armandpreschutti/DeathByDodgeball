using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSelectionPanelObserver : MonoBehaviour
{
    public PlayerInputManager inputManager;
    public int slotId;
    public bool isAvailible;
    public static Action<int> onEnableButton;
    public static Action<int> onDisableButton;
    public static Action<int> onUpdateButton;

    private void OnEnable()
    {
        PlayerSelectionPanelBroadcaster.onSelected += DisableAvailibility;
        PlayerSelectionPanelBroadcaster.onDeselected += EnableAvailibility;
    }
    private void OnDisable()
    {
        PlayerSelectionPanelBroadcaster.onSelected -= DisableAvailibility;
        PlayerSelectionPanelBroadcaster.onDeselected -= EnableAvailibility;
    }

    public void EnableAvailibility(int id)
    {
        if(id == slotId)
        {
            isAvailible = true;
            onEnableButton?.Invoke(slotId);
        }
    }

    public void DisableAvailibility(int id)
    {
        if (id == slotId)
        {
            isAvailible = false;
            onDisableButton?.Invoke(slotId);
        }
    }

}
