using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [Header("Components")]
    public PlayerManager playerManager;

    [Header("Actions")]
    public InputAction moveAction;
    [SerializeField] InputAction fireAction;
    [SerializeField] InputAction catchAction;
    [SerializeField] InputAction dashAction;
    [SerializeField] InputAction readyAction;


    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        fireAction = playerManager.playerInput.actions["Fire"];
        catchAction = playerManager.playerInput.actions["Catch"];
        moveAction = playerManager.playerInput.actions["Move"];
        dashAction = playerManager.playerInput.actions["Dash"];
        readyAction = playerManager.playerInput.actions["Ready"];
    }
    private void OnEnable()
    {
        fireAction.started += StartAiming;
        fireAction.canceled += StopAiming;
        catchAction.performed += Catch;
        dashAction.performed += Dodge;
        readyAction.performed += Ready;
    }

    private void OnDisable()
    {
        fireAction.started -= StartAiming;
        fireAction.canceled -= StopAiming;
        catchAction.performed -= Catch;
        dashAction.performed -= Dodge;
        readyAction.performed -= Ready;
    }
    public void StartAiming(InputAction.CallbackContext context)
    {
        Debug.Log($"{name} Started Aiming pressed");
        playerManager.meleeHandler.StartAiming();
    }
    public void StopAiming(InputAction.CallbackContext context)
    {
        Debug.Log($"{name} Stop Aiming pressed");
        playerManager.meleeHandler.StopAiming();
    }
    public void Catch(InputAction.CallbackContext context)
    {
        Debug.Log($"{name} Catch pressed");
        playerManager.meleeHandler.Catch();
    }
    public void Dodge(InputAction.CallbackContext context)
    {
        Debug.Log($"{name} Dodge pressed");
        playerManager.locomotionHandler.Dodge();
    }
    public void Ready(InputAction.CallbackContext context)
    {
        Debug.Log($"{name} Ready pressed");
        playerManager.Ready();
    }
}
