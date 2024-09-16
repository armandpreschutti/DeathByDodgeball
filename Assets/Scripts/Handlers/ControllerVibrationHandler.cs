using JetBrains.Annotations;
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

    public float ExplosionRumbleTime;
    public float ExplosionLowIntensity;
    public float ExplosionHighIntensity;

    public float SuperLowIntensity;
    public float SuperHighIntensity;

    public float AimLowIntensity;
    public float AimHighIntensity;

    public float CatchRumbleTime;
    public float CatchLowIntensity;
    public float CatchHighIntensity;

    public float ContactRumbleTime;
    public float ContactLowIntensity;
    public float ContactHighIntensity;

    public float DeathRumbleTime;
    public float DeathLowIntensity;
    public float DeathHighIntensity;

    public float ErrorRumbleTime;
    public float ErrorLowIntesitiy;
    public float ErrorHighIntesitiy;

    private void Awake()
    {
        _playerManager = GetComponent<PlayerManager>();
        _userController = GetComponent<UserController>();
        _playerInput = GetComponent<PlayerInput>();
       // playerId = _playerManager._playerId;
    }

    private void OnEnable()
    {
        BallManager.onExplosion += PerformExplosionRumble;
        PawnManager.onPlayerLoaded += SubscribeToStateMachine;
        PawnManager.onPlayerUnloaded += UnsubscribeFromStateMachine;
        _userController.onInputError += ErrorRumble;
        PauseMenuController.OnGamePaused += StopAllRumble;

    }

    private void OnDisable()
    {
        BallManager.onExplosion -= PerformExplosionRumble;
        PawnManager.onPlayerLoaded -= SubscribeToStateMachine;
        PawnManager.onPlayerUnloaded -= UnsubscribeFromStateMachine;
        _userController.onInputError -= ErrorRumble;
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

    public void PerformDodgeRumble(bool value)
    {
        if(value)
        {
            gamepad.SetMotorSpeeds(DodgeLowIntensity, DodgeHighIntensity);
            StartCoroutine(StopRumble(DodgeRumbleTime));
        }

    }

    public void PerformExplosionRumble()
    {
        gamepad.SetMotorSpeeds(ExplosionLowIntensity, ExplosionHighIntensity);
        StartCoroutine(StopRumble(ExplosionRumbleTime));
    }

    public void PerformSuperRumble(bool value)
    {
        if (value)
        {
            gamepad.SetMotorSpeeds(SuperLowIntensity, SuperHighIntensity);
        }
        else
        {
            gamepad.SetMotorSpeeds(0f, 0f);
        }
    }

    public void PerformAimRumble(bool value)
    {
        if (value)
        {
            gamepad.SetMotorSpeeds(AimLowIntensity, AimHighIntensity);
        }
        else
        {
            gamepad.SetMotorSpeeds(0f, 0f);
        }
    }

    public void PerformCatchRumble()
    {
        gamepad.SetMotorSpeeds(CatchLowIntensity, CatchHighIntensity);
        StartCoroutine(StopRumble(CatchRumbleTime));
    }

    public void PerformContactRumble()
    {
        gamepad.SetMotorSpeeds(ContactLowIntensity, ContactHighIntensity);
        StartCoroutine(StopRumble(ContactRumbleTime));
    }

    public void PerformDeathRumble(bool value)
    {
        if(value)
        {
            gamepad.SetMotorSpeeds(DeathLowIntensity, DeathHighIntensity);
            StartCoroutine(StopRumble(DeathRumbleTime));
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

    public void SubscribeToStateMachine(int slotId, int pId, GameObject stateMachine)
    {
        gamepad = _playerInput.devices.OfType<Gamepad>().FirstOrDefault();
        if (gamepad == null)
        {
            //this.enabled = false;
            Destroy(this);
        }
        if (pId == playerId)
        {
            //Debug.LogWarning($"Player {pId} subscribed to vibrations");
            _playerStateMachine = stateMachine.GetComponent<PlayerStateMachine>();
            _playerStateMachine.OnDodge += PerformDodgeRumble;
            _playerStateMachine.OnSuperState += PerformSuperRumble;
            _playerStateMachine.OnAim += PerformAimRumble;
            _playerStateMachine.OnBallCaught += PerformCatchRumble;
            _playerStateMachine.OnBallContact += PerformContactRumble;
            _playerStateMachine.OnDeath += PerformDeathRumble;
        }
    }
    public void UnsubscribeFromStateMachine(int slotId, int pId, GameObject stateMachine)
    {
        if (pId == playerId)
        {
            //Debug.LogWarning($"Player {pId} unsubscribed from vibrations");
            _playerStateMachine = stateMachine.GetComponent<PlayerStateMachine>();
            _playerStateMachine.OnDodge -= PerformDodgeRumble;
            _playerStateMachine.OnSuperState -= PerformSuperRumble;
            _playerStateMachine.OnAim -= PerformAimRumble;
            _playerStateMachine.OnBallCaught -= PerformCatchRumble;
            _playerStateMachine.OnBallContact -= PerformContactRumble;
            _playerStateMachine.OnDeath -= PerformDeathRumble;
        }
    }
}
