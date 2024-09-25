using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PawnManager : MonoBehaviour
{
    public int slotId;
    public int skinId;
    public int playerId;
    public int teamId;
    public string playerName;
    public Color pawnColor;
    public bool isEliminated;
    public PlayerStateMachine playerStateMachine;
    public bool isCpu;
   // public TextMeshProUGUI playerTag;
   // public SpriteRenderer spriteRenderer;
    public static Action<int, int, GameObject> onPlayerLoaded;
    public static Action<int, int, GameObject> onPlayerUnloaded;
    public static Action<int> onRespawn;


    private void Awake()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
        playerName = playerId != 0 ? $"P{playerId}" : "CPU";
        isCpu = playerName == "CPU" ? true : false;
        if(isCpu)
        {
            GameObject instance = gameObject;
            instance.AddComponent<CPUBrain>();
        }

     //   playerTag.text = playerName;
    }

    private void OnEnable()
    {
        playerStateMachine.OnRespawn += BroadcastRespawn;
        MatchInstanceManager.onEnablePawnControl += EnableStateMachine;
        MatchInstanceManager.onEndMatch += DisableStateMachine;
    }
    private void OnDisable()
    {
        playerStateMachine.OnRespawn -= BroadcastRespawn;
        MatchInstanceManager.onEnablePawnControl -= EnableStateMachine;
        MatchInstanceManager.onEndMatch -= DisableStateMachine;
    }
    public void OnDestroy()
    {
        onPlayerUnloaded?.Invoke(slotId, playerId, gameObject);
    }

    private void Start()
    {
        onPlayerLoaded?.Invoke(slotId, playerId, gameObject);
        if (GameManager.gameInstance.isDebugging)
        {
            playerStateMachine.enabled = true;
        }
    }

    public void BroadcastRespawn()
    {
        onRespawn?.Invoke(slotId);
    }

    public void EnableStateMachine()
    {
        playerStateMachine.enabled = true;
    }

    public void DisableStateMachine()
    {
        playerStateMachine.enabled = false;
    }

    
}
