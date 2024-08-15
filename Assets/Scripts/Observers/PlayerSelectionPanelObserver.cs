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
    public static Action<int> onEnableButton;
    public static Action<int> onDisableButton;
    public static Action<int> onUpdateButton;
    public bool isFilled;

    private void OnEnable()
    {
        PlayerSelectionPanelBroadcaster.onSelected += DisableAvailibility;
        PlayerSelectionPanelBroadcaster.onDeselected += EnableAvailibility;
        PlayerSelectionManager.onSetMatchSlot += PreviewMatchConfigurationSlot;
    }
    private void OnDisable()
    {
        PlayerSelectionPanelBroadcaster.onSelected -= DisableAvailibility;
        PlayerSelectionPanelBroadcaster.onDeselected -= EnableAvailibility;
        PlayerSelectionManager.onSetMatchSlot -= PreviewMatchConfigurationSlot;
    }

    public void EnableAvailibility(int id)
    {
        if(id == slotId && !isFilled)
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
            playerTagText.text = $"P{playerId}";
            isFilled = true;
        }
        else
        {
            if(isAvailible)
            {
                promptText.text = "Add AI";
            }

        }

    }

}
