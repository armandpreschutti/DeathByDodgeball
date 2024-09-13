using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [Header("Stats")]
    public int _playerId;
    public int _skinId;
    public int _slotId;

    [Header("Components")]
    public PlayerInput playerInput;
    public UserController userController;
    public ControllerVibrationHandler controllerVibrationHandler;
    public static Action<int, GameObject> onJoin;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SetSceneState;
        MatchInstanceManager.onInitializeMatchInstance += EnableInputVibration;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SetSceneState;
        MatchInstanceManager.onInitializeMatchInstance -= EnableInputVibration;
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        BroadcastJoin();
        SetComponents();
    }
    
    public void BroadcastJoin()
    {
        onJoin?.Invoke(_playerId, gameObject);
    }

    public void SetComponents()
    {
        playerInput= GetComponent<PlayerInput>();
        userController= GetComponent<UserController>();
        if (GetComponent<ControllerVibrationHandler>() != null)
        {
            controllerVibrationHandler = GetComponent<ControllerVibrationHandler>();
        }
    }
    public void EnableInputVibration()
    {
        if(SceneManager.GetActiveScene().name == "MatchInstance" || SceneManager.GetActiveScene().name == "Debug")
        {
            if (controllerVibrationHandler != null)
            {
                controllerVibrationHandler.playerId = _playerId;
                controllerVibrationHandler.enabled = true;
            }
        }
        

    }
    public void SetSceneState(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "PostMatch")
        {
            userController.enabled = false;
            playerInput.enabled = false;
           // controllerVibrationHandler.enabled = false;
        }
        else if (SceneManager.GetActiveScene().name == "MatchInstance")
        {
            userController.enabled = true;
            playerInput.enabled = true;
        }
    }
}
