using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

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
    public int selectedSlot;
    public int currentSkin;
    public bool isPlayerSelected;
    public Action onCycleNextSkin;
    public Action onCyclePreviousSkin;
    public static Action<int, int, int> onRemoveSelection;
    public static Action<int, int, int, int> onAddAi;
    public static Action<int, int, int> onSubmit;
    public static Action<int, int, int> onSubmitAi;
    public static Action onInitiateMatchStart;
    public static Action onDestroyAllPlayers;
    public bool canSelectAI;
    public bool isControlEnabled;

    private void Awake()
    {
        SetInitialPlayerValues();   
    }

    private void OnEnable()
    {
        PlayerSelectionManager.onResetPlayer += ResetPlayer;
        PlayerSelectionPanelBroadcaster.onSelected += SetCurrentSelection;
        playerInput.actions["NextSkin"].performed += OnNextSkinPerformed;
        playerInput.actions["PreviousSkin"].performed += OnPreviousSkinPerformed;
        playerInput.actions["JoinGame"].performed += OnAddAIPerformed;
        playerInput.actions["Remove"].performed += OnRemoveSelectionPerformed;
        playerInput.actions["Start"].performed += OnStartPerformed;
    }

    private void OnDisable()
    {
        PlayerSelectionManager.onResetPlayer -= ResetPlayer;
        PlayerSelectionPanelBroadcaster.onSelected -= SetCurrentSelection;
        playerInput.actions["NextSkin"].performed -= OnNextSkinPerformed;
        playerInput.actions["PreviousSkin"].performed -= OnPreviousSkinPerformed;
        playerInput.actions["JoinGame"].performed -= OnAddAIPerformed;
        playerInput.actions["Remove"].performed -= OnRemoveSelectionPerformed;
        playerInput.actions["Start"].performed -= OnStartPerformed;
    }

    private void OnNextSkinPerformed(InputAction.CallbackContext ctx)
    {
        CycleNextSkin();
    }

    private void OnPreviousSkinPerformed(InputAction.CallbackContext ctx)
    {
        CyclePreviousSkin();
    }

    private void OnAddAIPerformed(InputAction.CallbackContext ctx)
    {
        AddAI();
    }

    private void OnRemoveSelectionPerformed(InputAction.CallbackContext ctx)
    {
        RemoveSelection();
    }

    private void OnStartPerformed(InputAction.CallbackContext ctx)
    {
        InitiateMatchStart();
    }

    public void DisableConfigurationController(Scene scene)
    {
        if(scene.name == "PlayerSelection")
        {
            Destroy(this.gameObject);
        }
    }

    public void DestroyAllPlayers()
    {
        onDestroyAllPlayers?.Invoke();
    }

    public void SetInitialPlayerValues()
    {
        transform.parent.name = $"Player{playerId}";
        playerSelectionManager = FindObjectOfType<PlayerSelectionManager>();
        playerInput = GetComponentInParent<PlayerInput>();
        playerId = playerInput.playerIndex + 1 ;
        Slot1.name = $"P{playerId}Button1";
        Slot2.name = $"P{playerId}Button2";
        Slot3.name = $"P{playerId}Button3";
        Slot4.name = $"P{playerId}Button4";
        eventSystem.playerRoot = gameObject;
        inputModule.actionsAsset = playerInput.actions;
        GetButtonAvailibility();
        SetInitialSlot();
        transform.parent.name = $"Player{playerId}";
    }

    public void InitiateMatchStart()
    {
       // Debug.Log($"P{playerId} has pressed the 'start' button");
        onInitiateMatchStart?.Invoke();
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
            if (availibleSlots[i].isAvailible && !availibleSlots[i].isFilled)
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
            selectedSlot = currentSlot;
        }
        else if(canSelectAI)
        {
            int oldId = playerId;
            playerId = 0;
            onSubmitAi?.Invoke(playerId, currentSlot, currentSkin);
            playerId = oldId;
            canSelectAI = false;
        }
    }

    public void AddAI()
    {
        if (isPlayerSelected)
        {
            canSelectAI = true;
            currentSkin = 0;
            onAddAi?.Invoke(playerId, currentSlot, currentSkin, selectedSlot);
        }
    }
    public void RemoveSelection()
    {
        onRemoveSelection?.Invoke(playerId,currentSlot,currentSkin);
    }
    public void ResetPlayer(int id, bool selection)
    {
        if (playerId == id)
        {
            isPlayerSelected = selection;
        }
        else if(id == 0)
        {
            canSelectAI = selection;
        }
    }
    public void CycleNextSkin()
    {      
        onCycleNextSkin?.Invoke();
    }

    public void CyclePreviousSkin()
    {
        onCyclePreviousSkin?.Invoke();
    }
}
