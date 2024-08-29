using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserController : MonoBehaviour
{
    public int playerId;
    public PlayerManager playerManager;
    public PlayerStateMachine playerStateMachine;
    public bool isActivated;

    private void Awake()
    { 
        playerManager= GetComponent<PlayerManager>();      
    }

    private void OnEnable()
    {
        MatchInstanceManager.onInitializeMatchInstance += SetStateMachine;
        MatchInstanceManager.onEnablePawnControl += EnableControl;
        MatchInstanceManager.onDisablePawnControl += DisableControl;
        SubscribeToActions();
    }

    private void OnDisable()
    {
        MatchInstanceManager.onInitializeMatchInstance -= SetStateMachine;
        MatchInstanceManager.onEnablePawnControl -= EnableControl;
        MatchInstanceManager.onDisablePawnControl -= DisableControl;
        UnsubscribeFromActions();
    }

    public void SetStateMachine()
    {
        playerId = playerManager._playerId;
        playerStateMachine = GameObject.Find($"PlayablePawn{playerId}(Clone)").GetComponent<PlayerStateMachine>();
        playerManager.playerInput.SwitchCurrentActionMap("Gameplay");
    }

    public void SetMoveInput(Vector2 value)
    {
        if(playerStateMachine != null && isActivated)
        {
            //Debug.Log("Move called on controller");
            if (!playerStateMachine.IsDodging)
            {
                playerStateMachine.MoveInput = value;
            }
        }     
    }

    public void SetAimInput(Vector2 value)
    {
        if(playerStateMachine != null && isActivated)
        {
            //Debug.Log("Aim called on controller");
            playerStateMachine.AimInput = value;
        }        
    }

    public void SetDodgeInput(bool value)
    {
        if (playerStateMachine != null && isActivated)
        {
            //Debug.Log("Dodge called on controller");
            if (playerStateMachine.MoveInput != Vector2.zero && !playerStateMachine.IsDodging && !playerStateMachine.IsCatching && !playerStateMachine.IsHurt && !playerStateMachine.IsThrowing && !playerStateMachine.IsAiming)
            {
                playerStateMachine.IsDodgePressed = value;
            }
        }
    }
    
    public void SetCatchInput(bool value)
    {
        if (playerStateMachine != null && isActivated)
        {
            //Debug.Log("Catch called on controller");
            if (!playerStateMachine.IsCatching && !playerStateMachine.IsHurt && !playerStateMachine.IsThrowing)
            {
                playerStateMachine.IsCatchPressed = value;
            }
        }
    }

    public void SetThrowInput(bool value)
    {
        if (playerStateMachine != null && isActivated)
        {
            //Debug.Log("Throw called on controller");
            if (!playerStateMachine.IsDodging && !playerStateMachine.IsHurt && !playerStateMachine.IsThrowing && playerStateMachine.IsEquipped)
            {
                playerStateMachine.IsThrowPressed = value;
            }
        }
    }
    public void SubscribeToActions()
    {
        playerManager.playerInput.actions["Move"].performed += ctx => SetMoveInput(ctx.ReadValue<Vector2>());
        playerManager.playerInput.actions["Aim"].performed += ctx => SetAimInput(ctx.ReadValue<Vector2>());
        playerManager.playerInput.actions["Dodge"].performed += ctx => SetDodgeInput(ctx.ReadValueAsButton());
        playerManager.playerInput.actions["Catch"].performed += ctx => SetCatchInput(ctx.ReadValueAsButton());
        playerManager.playerInput.actions["Throw"].performed += ctx => SetThrowInput(ctx.ReadValueAsButton());
        playerManager.playerInput.actions["Throw"].canceled += ctx => SetThrowInput(ctx.ReadValueAsButton());
    }
    public void UnsubscribeFromActions()
    {
        playerManager.playerInput.actions["Move"].performed -= ctx => SetMoveInput(ctx.ReadValue<Vector2>());
        playerManager.playerInput.actions["Aim"].performed -= ctx => SetAimInput(ctx.ReadValue<Vector2>());
        playerManager.playerInput.actions["Dodge"].performed -= ctx => SetDodgeInput(ctx.ReadValueAsButton());
        playerManager.playerInput.actions["Catch"].performed -= ctx => SetCatchInput(ctx.ReadValueAsButton());
        playerManager.playerInput.actions["Throw"].performed -= ctx => SetThrowInput(ctx.ReadValueAsButton());
        playerManager.playerInput.actions["Throw"].canceled -= ctx => SetThrowInput(ctx.ReadValueAsButton());
    }

    public void EnableControl()
    {
        isActivated = true;
    }
    public void DisableControl()
    {
        isActivated = false;
    }


}
