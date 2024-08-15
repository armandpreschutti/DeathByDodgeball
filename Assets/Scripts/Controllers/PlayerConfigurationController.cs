using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerConfigurationController : MonoBehaviour
{
    public PlayerInput playerInput;
    public PlayerSelectionManager playerSelectionManager;
    public int playerId;
    public MultiplayerEventSystem eventSystem;
    public InputSystemUIInputModule inputModule;
    public GameObject Slot1;
    public GameObject Slot2;
    public GameObject Slot3;
    public GameObject Slot4;
    public PlayerSelectionPanelObserver[] availibleSlots;
    public int currentSlot;
    public int currentSkin;
    public bool isPlayerSelected;
    public Action onCycleNextSkin;
    public Action onCyclePreviousSkin;
    public static Action<int, int, int> onSubmit;
    public static Action<int> onAddAi;
    public static Action onSubmitAi;

    //public static Action onNewPlayer;

    private void Awake()
    {
        SetInitialPlayerValues();   
    }

    private void OnEnable()
    {
        //SetInitialPlayerValues();
        PlayerSelectionPanelBroadcaster.onSelected += SetCurrentSelection;
        playerInput.actions["NextSkin"].performed += ctx => CycleNextSkin();
        playerInput.actions["PreviousSkin"].performed += ctx => CyclePreviousSkin();
        //playerInput.actions["JoinGame"].performed += ctx => AddAI();
        Slot1.GetComponent<PlayerSelectionPanelBroadcaster>().onButtonClicked += Submit;
        Slot2.GetComponent<PlayerSelectionPanelBroadcaster>().onButtonClicked += Submit;
        Slot3.GetComponent<PlayerSelectionPanelBroadcaster>().onButtonClicked += Submit;
        Slot4.GetComponent<PlayerSelectionPanelBroadcaster>().onButtonClicked += Submit;
        
    }
    private void OnDisable()
    {
        PlayerSelectionPanelBroadcaster.onSelected -= SetCurrentSelection;
        playerInput.actions["NextSkin"].performed -= ctx => CycleNextSkin();
        playerInput.actions["PreviousSkin"].performed -= ctx => CyclePreviousSkin();
       // playerInput.actions["JoinGame"].performed -= ctx => AddAI();
        Slot1.GetComponent<PlayerSelectionPanelBroadcaster>().onButtonClicked -= Submit;
        Slot2.GetComponent<PlayerSelectionPanelBroadcaster>().onButtonClicked -= Submit;
        Slot3.GetComponent<PlayerSelectionPanelBroadcaster>().onButtonClicked -= Submit;
        Slot4.GetComponent<PlayerSelectionPanelBroadcaster>().onButtonClicked -= Submit;
    }

    public void SetInitialPlayerValues()
    {
        playerSelectionManager = FindObjectOfType<PlayerSelectionManager>();
        playerInput = GetComponent<PlayerInput>();
        playerId = playerInput.playerIndex + 1 ;
        Slot1.name = $"P{playerId}Button1";
        Slot2.name = $"P{playerId}Button2";
        Slot3.name = $"P{playerId}Button3";
        Slot4.name = $"P{playerId}Button4";
        eventSystem.playerRoot = gameObject;
        inputModule.actionsAsset = playerInput.actions;
        GetButtonAvailibility();
        SetInitialSlot();
    }

    public void GetButtonAvailibility()
    {
        availibleSlots = FindObjectsOfType<PlayerSelectionPanelObserver>();

        // Sort the array based on the name property
        Array.Sort(availibleSlots, (a, b) => a.name.CompareTo(b.name));
    }

    private void SetCurrentSelection(int id)
    {
        if(eventSystem.currentSelectedGameObject != null)
        {
            currentSlot = GetSlotNumber(eventSystem.currentSelectedGameObject.gameObject.name);
        }
    }

    public void SetSelection()
    {
        Debug.Log(eventSystem.currentSelectedGameObject.gameObject.name);
    }

    public void SetInitialSlot()
    {
        for(int i = 0;i < availibleSlots.Length; i++)
        {
            if (availibleSlots[i].isAvailible)
            {
                eventSystem.firstSelectedGameObject = GameObject.Find($"P{playerId}Button{i +1}");
                return;
            }
        }
    }

    public int GetSlotNumber(string input)
    {
        // Define the regular expression pattern to find all numbers in the string
        Regex regex = new Regex(@"\d+");
        MatchCollection matches = regex.Matches(input);

        // Check if there are any matches and return the last one
        if (matches.Count > 0)
        {
            // Get the last match's value
            string lastNumberString = matches[matches.Count - 1].Value;
            return int.Parse(lastNumberString);
        }

        // Return -1 or another value indicating no number was found
        return -1;
    }

    public void Submit()
    {
        if (!isPlayerSelected)
        {
            isPlayerSelected = true;
            onSubmit?.Invoke(playerId, currentSlot, currentSkin);
        }
    }

   /* public void AddAI()
    {
        if(isPlayerSelected)
        {
            onAddAi?.Invoke(currentSlot);
        }

    }
*/
    public void CycleNextSkin()
    {
        //Debug.Log($"Player {playerID} is cycling next skin");        
        onCycleNextSkin?.Invoke();
    }

    public void CyclePreviousSkin()
    {
        //Debug.Log($"Player {playerID} is cycling previous skin");
        onCyclePreviousSkin?.Invoke();
    }
}
