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
    public int playerID;
    public MultiplayerEventSystem eventSystem;
    public InputSystemUIInputModule inputModule;
    public GameObject Slot1;
    public GameObject Slot2;
    public GameObject Slot3;
    public GameObject Slot4;
    public PlayerSelectionPanelObserver[] availibleSlots;
    public int currentSlot;
    //public static Action onNewPlayer;

    private void Awake()
    {
        SetInitialPlayerValues();   
    }

    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }

    public void SetInitialPlayerValues()
    {
        playerSelectionManager = FindObjectOfType<PlayerSelectionManager>();
        playerInput = GetComponent<PlayerInput>();
        playerID = playerInput.playerIndex + 1 ;
        Slot1.name = $"P{playerID}Button1";
        Slot2.name = $"P{playerID}Button2";
        Slot3.name = $"P{playerID}Button3";
        Slot4.name = $"P{playerID}Button4";
        //eventSystem.firstSelectedGameObject = GameObject.Find($"P{playerID}Button{playerID}");

        eventSystem.playerRoot = gameObject;
        inputModule.actionsAsset = playerInput.actions;
        GetButtonAvailibility();
        SetInitialSlot();
        // onNewPlayer?.Invoke();
    }

    public void GetButtonAvailibility()
    {
        availibleSlots = FindObjectsOfType<PlayerSelectionPanelObserver>();

        // Sort the array based on the name property
        Array.Sort(availibleSlots, (a, b) => a.name.CompareTo(b.name));
    }
    private void Update()
    {
        currentSlot = GetSlotNumber(eventSystem.currentSelectedGameObject.gameObject.name);
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
                eventSystem.firstSelectedGameObject = availibleSlots[i].gameObject;
            }
        }
        eventSystem.firstSelectedGameObject = GameObject.Find($"P{playerID}Button{playerID}");
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
}
