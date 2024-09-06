using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int slotId;
    public int maxLives;
    public int currentLives;
    public bool isEliminated;
    public PlayerStateMachine playerStateMachine;
    public PawnManager pawnManager;
    public static Action<int, int> onPlayerDeath;
    public static Action<int> onPlayerElimination;
    public static Action<int, string, HealthSystem> onHealthInitialized;

    private void Awake()
    {
        if(GameManager.gameInstance.isDebugging)
        {
            this.enabled = false;
        }
        playerStateMachine = GetComponent<PlayerStateMachine>();
        pawnManager = GetComponent<PawnManager>();
        slotId = pawnManager.slotId;
    }
    private void OnEnable()
    {
        if(playerStateMachine!= null)
        {
            playerStateMachine.OnDeath += RemoveLife;
        }

    }
    private void OnDisable()
    { 
        if(playerStateMachine!= null)
        {
            playerStateMachine.OnDeath -= RemoveLife;
        }

    }
    private void Start()
    {
        currentLives = maxLives;
        onHealthInitialized?.Invoke(slotId, pawnManager.playerName, this);
    }

    public void RemoveLife(bool value)
    {
        if(value)
        {
            currentLives--;
            onPlayerDeath?.Invoke(slotId, currentLives);
            if (currentLives <= 0)
            {
                playerStateMachine.CanRespawn = false;
                isEliminated = true;
                onPlayerElimination?.Invoke(slotId);
            }
        }

    } 
}
