
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserController : MonoBehaviour
{
    public int playerId;
    public PlayerManager playerManager;
    public PlayerStateMachine playerStateMachine;
    public bool isActivated;
    public static Action onPausePressed;
    public Action onInputError;

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
        if(playerStateMachine != null && isActivated && !GameManager.gameInstance.isPaused)
        {
            //Debug.Log("Move called on controller");
            if (/*!playerStateMachine.IsDodging &&*/ !playerStateMachine.IsDead)
            {
                playerStateMachine.MoveInput = value;
            }
            else
            {
                playerStateMachine.MoveInput = Vector2.zero;
            }
        }
        else
        {
            playerStateMachine.MoveInput = Vector2.zero;
        }
    }

    public void SetAimInput(Vector2 value)
    {
        if(playerStateMachine != null && isActivated && !GameManager.gameInstance.isPaused)
        {
            if (!playerStateMachine.IsDead)
            {
                //Debug.Log("Aim called on controller");
                playerStateMachine.AimInput = value;
            }
        }        
    }

    public void SetDodgeInput(bool value)
    {
        if (playerStateMachine != null && isActivated && !GameManager.gameInstance.isPaused)
        {
            //Debug.Log("Dodge called on controller");
            if (playerStateMachine.MoveInput != Vector2.zero && !playerStateMachine.IsDodging && !playerStateMachine.IsCatching && !playerStateMachine.IsDead && !playerStateMachine.IsThrowing)
            {
                if (!playerStateMachine.IsExhausted)
                {
                    playerStateMachine.IsDodgePressed = value;
                }
                else
                {
                    onInputError?.Invoke();
                }
            }
        }
    }
    
    public void SetCatchInput(bool value)
    {
        if (playerStateMachine != null && isActivated && !GameManager.gameInstance.isPaused)
        {
            //Debug.Log("Catch called on controller");
            if (!playerStateMachine.IsCatching && !playerStateMachine.IsHurt && !playerStateMachine.IsThrowing && !playerStateMachine.IsDead)
            {
                playerStateMachine.IsCatchPressed = value;
            }
        }
    }

    public void SetThrowInput(bool value)
    {
        if (playerStateMachine != null && isActivated && !GameManager.gameInstance.isPaused)
        {
            //Debug.Log("Throw called on controller");
            if (!playerStateMachine.IsDodging && !playerStateMachine.IsHurt && !playerStateMachine.IsThrowing && playerStateMachine.IsEquipped)
            {
                playerStateMachine.IsThrowPressed = value;
            }
        }
    }

    public void SetPauseInput(bool value)
    {
        onPausePressed?.Invoke();
        
    }
    public void SubscribeToActions()
    {
        playerManager.playerInput.actions["Move"].performed += OnMovePerformed;
        playerManager.playerInput.actions["Aim"].performed += OnAimPerformed;
        playerManager.playerInput.actions["Dodge"].performed += OnDodgePerformed;
        playerManager.playerInput.actions["Catch"].performed += OnCatchPerformed;
        playerManager.playerInput.actions["Throw"].performed += OnThrowPerformed;
        playerManager.playerInput.actions["Throw"].canceled += OnThrowCanceled;
        playerManager.playerInput.actions["Pause"].performed += OnPausePerformed;
    }

    public void UnsubscribeFromActions()
    {
        playerManager.playerInput.actions["Move"].performed -= OnMovePerformed;
        playerManager.playerInput.actions["Aim"].performed -= OnAimPerformed;
        playerManager.playerInput.actions["Dodge"].performed -= OnDodgePerformed;
        playerManager.playerInput.actions["Catch"].performed -= OnCatchPerformed;
        playerManager.playerInput.actions["Throw"].performed -= OnThrowPerformed;
        playerManager.playerInput.actions["Throw"].canceled -= OnThrowCanceled;
        playerManager.playerInput.actions["Pause"].performed -= OnPausePerformed;
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        SetMoveInput(ctx.ReadValue<Vector2>());
    }

    private void OnAimPerformed(InputAction.CallbackContext ctx)
    {
        SetAimInput(ctx.ReadValue<Vector2>());
    }

    private void OnDodgePerformed(InputAction.CallbackContext ctx)
    {
        SetDodgeInput(ctx.ReadValueAsButton());
    }

    private void OnCatchPerformed(InputAction.CallbackContext ctx)
    {
        SetCatchInput(ctx.ReadValueAsButton());
    }

    private void OnThrowPerformed(InputAction.CallbackContext ctx)
    {
        SetThrowInput(ctx.ReadValueAsButton());
    }

    private void OnThrowCanceled(InputAction.CallbackContext ctx)
    {
        SetThrowInput(ctx.ReadValueAsButton());
    }

    private void OnPausePerformed(InputAction.CallbackContext ctx)
    {
        SetPauseInput(ctx.ReadValueAsButton());
    }

    public void EnableControl()
    {
        isActivated = true;
        playerStateMachine.MoveInput = Vector2.zero;
    }
    public void DisableControl()
    {
        isActivated = false;
        playerStateMachine.MoveInput = Vector2.zero;
    }


}
