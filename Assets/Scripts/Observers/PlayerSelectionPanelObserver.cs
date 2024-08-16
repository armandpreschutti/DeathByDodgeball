using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSelectionPanelObserver : MonoBehaviour
{
    public PreviewSkinsSO previewSkins;
    public PlayerInputManager inputManager;
    public int slotId;
    public bool isAvailible;
    public Image panel;
    public Image previewImage;
    public Image buttonImage;
    public TextMeshProUGUI promptText;
    public TextMeshProUGUI playerTagText;
    public Sprite selectedSprite;
    public Sprite unselectedSprite;
    public Sprite emptyImage;
    public static Action<int> onEnableButton;
    public static Action<int> onDisableButton;
    public static Action<int, bool> onUpdateButton;
    public bool isFilled;

    private void OnEnable()
    {
        PlayerSelectionPanelBroadcaster.onJoinSelection += SendAvailibility;
        PlayerSelectionPanelBroadcaster.onSelected += DisableAvailibility;
        PlayerSelectionPanelBroadcaster.onDeselected += EnableAvailibility;
        PlayerSelectionManager.onSetMatchSlot += PreviewMatchConfigurationSlot;
        PlayerSelectionManager.onRemoveMatchSlot += RemoveMatchConfigurationSlot;
    }
    private void OnDisable()
    {
        PlayerSelectionPanelBroadcaster.onJoinSelection -= SendAvailibility;
        PlayerSelectionPanelBroadcaster.onSelected -= DisableAvailibility;
        PlayerSelectionPanelBroadcaster.onDeselected -= EnableAvailibility;
        PlayerSelectionManager.onSetMatchSlot -= PreviewMatchConfigurationSlot;
        PlayerSelectionManager.onRemoveMatchSlot -= RemoveMatchConfigurationSlot;
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

    public void PreviewMatchConfigurationSlot(int playerId, int matchSlot, int skinId)
    {
        if(matchSlot == slotId)
        {
            previewImage.sprite = previewSkins.skins[skinId];
            panel.sprite = selectedSprite;
            buttonImage.enabled = false;
            promptText.text = "Ready!";
            if(playerId != 0)
            {
                playerTagText.text = $"P{playerId}";
            }
            else
            {
                playerTagText.text = $"CPU";
            }

            isFilled = true;
        }
        else
        {
            if (!isFilled)
            {
                promptText.text = "Add AI";
            }


        }

    }
    public void SendAvailibility(int id)
    {
        onUpdateButton?.Invoke(slotId, isFilled);
    }

    public void RemoveMatchConfigurationSlot(int playerId, int matchSlot, int skinId)
    {
        if (matchSlot == slotId)
        {
            previewImage.sprite = emptyImage;
            panel.sprite = unselectedSprite;
            buttonImage.enabled = true;
            playerTagText.text = null;
            isFilled = false;

            SendAvailibility(slotId);
        }
        if (!isFilled)
        {
            promptText.text = "Add AI";
        }

    }
}
