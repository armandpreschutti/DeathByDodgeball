using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [Header("Stats")]
    public int _playerId;
    public int _teamId;
    public int _skinId;
    [SerializeField] Color _teamColor;

    [Header("Components")]
    [SerializeField] PlayerInput _playerInput;
    public GameObject playerInstance;
    public static Action<int, GameObject> onJoin;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        SetPlayerComponents();
    }

    public void ResetPlayer()
    {
        _playerInput.SwitchCurrentActionMap("Player");
    }

    
    public void SetPlayerComponents()
    {
        if(GetComponentInChildren<PlayerConfigurationController>() != null)
        {
            PlayerConfigurationController controller = GetComponentInChildren<PlayerConfigurationController>();
            _playerId = controller.playerId;
        }

        _playerInput = GetComponent<PlayerInput>();
        onJoin?.Invoke(_playerId, gameObject);
    }
}
