using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] int _playerId;
    [SerializeField] int _teamId;
    [SerializeField] int _skinId;
    [SerializeField] bool _isInvicible;
    [SerializeField] Color _teamColor;

    [Header("Components")]
    [SerializeField] PlayerInput _playerInput;
    public Animator anim;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SetPlayerComponents();
    }

    public void ResetPlayer()
    {
        _playerInput.SwitchCurrentActionMap("Player");
    }

    
    public void SetPlayerComponents()
    {
        _playerInput = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
    }
}
