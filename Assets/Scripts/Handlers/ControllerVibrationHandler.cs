using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerVibrationHandler : MonoBehaviour
{
    private Gamepad gamepad;
    public int playerId;
    [SerializeField] UserController _userController;
    [SerializeField] PlayerStateMachine _playerStateMachine;
    [SerializeField] PlayerManager _playerManager;
    [SerializeField] PlayerInput _playerInput;

    public float DodgeRumbleTime;
    public float DodgeLowIntensity;
    public float DodgeHighIntensity;

    public float ErrorRumbleTime;
    public float ErrorLowIntesitiy;
    public float ErrorHighIntesitiy;

    private void Awake()
    {
        _playerManager = GetComponent<PlayerManager>();
        _userController = GetComponent<UserController>();
        _playerInput = GetComponent<PlayerInput>();
        playerId = _playerManager._playerId;
        //_stateMachine = GameObject.Find($"PlayablePawn{_playerManager._playerId}(Clone)")/*.GetComponent<PlayerStateMachine>()*/;
        
    }

    private void OnEnable()
    {
        PawnManager.onPlayerLoaded += SetStateMachine;
        UserController.onInputError += ErrorRumble;
        PauseMenuController.OnGamePaused += StopAllRumble;

    }

    private void OnDisable()
    {
        PawnManager.onPlayerLoaded -= SetStateMachine;
        _playerStateMachine.OnDodge -= DodgeRumble;
        UserController.onInputError -= ErrorRumble;
        PauseMenuController.OnGamePaused -= StopAllRumble;
    }

    void Start()
    {
        gamepad = _playerInput.devices.OfType<Gamepad>().FirstOrDefault();
        if (gamepad == null)
        {
            //this.enabled = false;
            Destroy(this);
        }
    }

    public void DodgeRumble(bool value)
    {
        if(value)
        {
            gamepad.SetMotorSpeeds(DodgeLowIntensity, DodgeHighIntensity);
            StartCoroutine(StopRumble(DodgeRumbleTime));
        }

    }

    public void StopAllRumble(bool value)
    {
        gamepad.SetMotorSpeeds(0, 0);
    }

    public void ErrorRumble()
    {
        gamepad.SetMotorSpeeds(ErrorLowIntesitiy, ErrorHighIntesitiy);
        StartCoroutine(StopRumble(ErrorRumbleTime));
    }

    private IEnumerator StopRumble(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (gamepad != null)
        {
            gamepad.SetMotorSpeeds(0f, 0f);
        }
    }

    public void SetStateMachine(int slotId, int pId, GameObject stateMachine)
    {
        if(pId == playerId)
        {
            _playerStateMachine = stateMachine.GetComponent<PlayerStateMachine>();
            _playerStateMachine.OnDodge += DodgeRumble;
        }
    }
}
