using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserController : MonoBehaviour
{
    public PlayerInput playerInput;
    public PlayerManager playerManager;
    public PlayerStateMachine playerStateMachine;

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        playerManager = GetComponentInParent<PlayerManager>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    private void OnEnable()
    {
        SubscribeToActions();
    }
    private void OnDisable()
    {
        UnsubscribeFromActions();
    }
    private void Update()
    {
        
    }

    public void SetMoveInput(Vector2 value)
    {
       // Debug.Log("Move called on controller");
        if (!playerStateMachine.IsDodging)
        {
            playerStateMachine.MoveInput = value;
        }
    }

    public void SetAimInput(Vector2 value)
    {
        //Debug.Log("Aim called on controller");
        playerStateMachine.AimInput = value;
    }

    public void SetDodgeInput(bool value)
    {
        //Debug.Log("Dodge called on controller");
        if (playerStateMachine.MoveInput != Vector2.zero && !playerStateMachine.IsDodging && !playerStateMachine.IsCatching && !playerStateMachine.IsHurt && !playerStateMachine.IsThrowing && !playerStateMachine.IsAiming)
        {
            playerStateMachine.IsDodgePressed = value;
        }

    }
    
    public void SetCatchInput(bool value)
    {
        //Debug.Log("Catch called on controller");
        if(!playerStateMachine.IsCatching && !playerStateMachine.IsHurt && !playerStateMachine.IsThrowing)
        {
            playerStateMachine.IsCatchPressed = value;
        }

    }

    public void SetThrowInput(bool value)
    {
        //Debug.Log("Throw called on controller");
        if(!playerStateMachine.IsDodging && !playerStateMachine.IsHurt && !playerStateMachine.IsThrowing && playerStateMachine.IsEquipped)
        {
            playerStateMachine.IsThrowPressed = value;
        }

    }

    public void EnableUserControl()
    {

    }
    public void PauseUserControls()
    {

    }

    public void SubscribeToActions()
    {
        playerInput.actions["Move"].performed += ctx => SetMoveInput(ctx.ReadValue<Vector2>());
        playerInput.actions["Aim"].performed += ctx => SetAimInput(ctx.ReadValue<Vector2>());
        playerInput.actions["Dodge"].performed += ctx => SetDodgeInput(ctx.ReadValueAsButton());
        playerInput.actions["Catch"].performed += ctx => SetCatchInput(ctx.ReadValueAsButton());
        playerInput.actions["Throw"].performed += ctx => SetThrowInput(ctx.ReadValueAsButton());
        playerInput.actions["Throw"].canceled += ctx => SetThrowInput(ctx.ReadValueAsButton());
    }
    public void UnsubscribeFromActions()
    {
        playerInput.actions["Move"].performed -= ctx => SetMoveInput(ctx.ReadValue<Vector2>());
        playerInput.actions["Aim"].performed -= ctx => SetAimInput(ctx.ReadValue<Vector2>());
        playerInput.actions["Dodge"].performed -= ctx => SetDodgeInput(ctx.ReadValueAsButton());
        playerInput.actions["Catch"].performed -= ctx => SetCatchInput(ctx.ReadValueAsButton());
        playerInput.actions["Throw"].performed -= ctx => SetThrowInput(ctx.ReadValueAsButton());
        playerInput.actions["Throw"].canceled -= ctx => SetThrowInput(ctx.ReadValueAsButton());
    }
}
