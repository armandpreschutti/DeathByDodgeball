using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionPreviewObserver : MonoBehaviour
{
    public PreviewSkinsSO previewSkins;
    public PlayerConfigurationController playerConfigurationController;
    public int currentIndex;
    public Image previewImage;

    private void Awake()
    {
        previewImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        InitializeSkin();
        playerConfigurationController.onCycleNextSkin += CycleNextSkin;
        playerConfigurationController.onCyclePreviousSkin += CyclePreviousSkin;

    }

    private void OnDisable()
    {
        playerConfigurationController.onCycleNextSkin -= CycleNextSkin;
        playerConfigurationController.onCyclePreviousSkin -= CyclePreviousSkin;
    }

    public void CycleNextSkin()
    {
        if(currentIndex < previewSkins.skins.Length -1)
        {
            currentIndex++;
            playerConfigurationController.currentSkin = currentIndex;
            previewImage.sprite = previewSkins.skins[currentIndex];
            previewImage.SetNativeSize();
        }
        else if(currentIndex == previewSkins.skins.Length -1)
        {
            currentIndex = 0;
            playerConfigurationController.currentSkin = currentIndex;
            previewImage.sprite = previewSkins.skins[currentIndex];
            previewImage.SetNativeSize();
        }
    }

    public void CyclePreviousSkin()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            previewImage.sprite = previewSkins.skins[currentIndex];
            playerConfigurationController.currentSkin = currentIndex;
            previewImage.SetNativeSize();
        }
        else if (currentIndex == 0)
        {
            currentIndex = previewSkins.skins.Length - 1;
            playerConfigurationController.currentSkin = currentIndex;
            previewImage.sprite = previewSkins.skins[currentIndex];
            previewImage.SetNativeSize();
        }
    }

    public void InitializeSkin()
    {
        currentIndex = playerConfigurationController.currentSkin;
        previewImage.sprite = previewSkins.skins[currentIndex];
        previewImage.SetNativeSize();
    }
}
