using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PawnManager : MonoBehaviour
{
    public int slotId;
    public int skinId;
    public int playerId;
    public int teamId;
    public PlayerStateMachine playerStateMachine;

    public static Action<int, GameObject> onPlayerLoaded;


    private void Awake()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();    
    }

    private void Start()
    {
        onPlayerLoaded?.Invoke(slotId, gameObject);
    }
}