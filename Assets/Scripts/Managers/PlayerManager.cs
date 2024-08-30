using JetBrains.Annotations;
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
    public int _skinId;
    public int _slotId;

    [Header("Components")]
    public PlayerInput playerInput;
    public UserController userController;

    public static Action<int, GameObject> onJoin;

    private void OnEnable()
    {

    }
    private void OnDisable()
    {

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
    }

}
