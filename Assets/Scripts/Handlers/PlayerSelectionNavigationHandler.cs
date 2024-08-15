using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerSelectionNavigationHandler : MonoBehaviour
{
    public int slotId;
    public int playerId;
    public Button thisButton;
    public PlayerSelectionPanelObserver observer;
    public PlayerConfigurationController controller;
    
    public int leftNeighbor;
    public int rightNeighbor;

    public Sprite p1Sprite;
    public Sprite p2Sprite;
    public Sprite p3Sprite;
    public Sprite p4Sprite;

    private void Awake()
    {
        thisButton = GetComponent<Button>();
        observer = GameObject.Find($"Slot{slotId}Panel").GetComponent<PlayerSelectionPanelObserver>();
        controller = GetComponentInParent<PlayerConfigurationController>();
        playerId = controller.playerId;
        gameObject.SetActive(observer.isAvailible);
        SpriteState spriteState = thisButton.spriteState;
        spriteState.selectedSprite = PlayerIDSprite();
        thisButton.spriteState = spriteState;
    }

    private void OnEnable()
    {
        PlayerSelectionPanelObserver.onDisableButton += SetNeighbors;
    }
    private void OnDisable()
    {
        PlayerSelectionPanelObserver.onDisableButton -= SetNeighbors;
    }

    public void SetNeighbors(int id)
    {
        SetLeftNeighbor();
        SetRightNeighbor();
    }

    public void SetLeftNeighbor()
    {
        // Iterate backwards from the current slotId to find the nearest available left neighbor
        for (int i = 0 ; i <  slotId - 1; i++)
        {
            if (controller.availibleSlots[i] != null && controller.availibleSlots[i].isAvailible)
            {
                // Debug.Log($"P{controller.playerID} Button {slotId} wants button {i+ 1} to be left neighbor");
                
                leftNeighbor = i + 1;
                
                // Get the current Navigation settings of the button
                Navigation navigation = thisButton.navigation;

                // Set the mode to Explicit to manually assign navigation directions
                navigation.mode = Navigation.Mode.Explicit;

                // Set the selectOnUp to the button above
                navigation.selectOnLeft = GameObject.Find($"P{controller.playerId}PanelButton{i + 1}").GetComponent<Button>();

                // Apply the modified navigation back to the button
                thisButton.navigation = navigation;

            }
        }
    }

    public void SetRightNeighbor()
    {
        // Iterate backwards from the current slotId to find the nearest available left neighbor
        for (int i = controller.availibleSlots.Length -1; i > slotId -1  ; i--)
        {
            if (controller.availibleSlots[i] != null && controller.availibleSlots[i].isAvailible)
            {
                // Debug.Log($"P{controller.playerID} Button {slotId} wants button {i + 1} to be right neighbor");

                rightNeighbor = i + 1;
               
                // Get the current Navigation settings of the button
                Navigation navigation = thisButton.navigation;

                // Set the mode to Explicit to manually assign navigation directions
                navigation.mode = Navigation.Mode.Explicit;

                // Set the selectOnUp to the button above
                navigation.selectOnRight = GameObject.Find($"P{controller.playerId}PanelButton{i+1}").GetComponent<Button>();

                // Apply the modified navigation back to the button
                thisButton.navigation = navigation;
            }
        }
    }

    public Sprite PlayerIDSprite()
    {
        switch (controller.playerId)
        {
            case 1:
                return p1Sprite;
            case 2:
                return p2Sprite;
            case 3:
                return p3Sprite;
            case 4:
                return p4Sprite;
            default: 
                return null;
        }
    }
}
