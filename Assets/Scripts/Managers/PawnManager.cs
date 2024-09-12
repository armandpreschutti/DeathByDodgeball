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
    public string playerName;
    public bool isEliminated;
    public PlayerStateMachine playerStateMachine;

    public static Action<int, int, GameObject> onPlayerLoaded;
    public static Action<int> onRespawn;


    private void Awake()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
        playerName = playerId != 0 ? $"P{playerId}" : "CPU";
    }

    private void OnEnable()
    {
        playerStateMachine.OnRespawn += BroadcastRespawn;
        MatchInstanceManager.onEndMatch += DisableStateMachine;
    }
    private void OnDisable()
    {
        playerStateMachine.OnRespawn -= BroadcastRespawn;
        MatchInstanceManager.onEndMatch -= DisableStateMachine;
    }

    private void Start()
    {
        onPlayerLoaded?.Invoke(slotId, playerId, gameObject);
    }

    public void BroadcastRespawn()
    {
        onRespawn?.Invoke(slotId);
    }

    public void DisableStateMachine()
    {
        playerStateMachine.enabled = false;
    }
}
