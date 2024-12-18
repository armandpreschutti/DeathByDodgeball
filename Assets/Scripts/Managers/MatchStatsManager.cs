using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MatchStatsManager : MonoBehaviour
{
    public bool isMatchOver;

    private void OnEnable()
    {
        BallManager.onPlayerHit += LogPlayerHit;
        BallManager.onPlayerKill += LogPlayerKill;
        BallManager.onPlayerThrowAttempt += LogPlayerThrowAttempt;
        MatchInstanceManager.onStartMatch += ResetMatchStats;
        BallManager.onCaught += LogPlayerCatch;
        PlayerStateMachine.onDodged += LogPlayerDodge;
        MatchInstanceManager.onStopMatchElements += SetMatchOverState;
    }
    private void OnDisable()
    {
        BallManager.onPlayerHit -= LogPlayerHit;
        BallManager.onPlayerKill -= LogPlayerKill;
        BallManager.onPlayerThrowAttempt -= LogPlayerThrowAttempt;
        MatchInstanceManager.onStartMatch -= ResetMatchStats;
        BallManager.onCaught -= LogPlayerCatch;
        PlayerStateMachine.onDodged -= LogPlayerDodge;
        MatchInstanceManager.onStopMatchElements -= SetMatchOverState;
    }

    public void SetMatchOverState()
    {
        isMatchOver = true;
    }

    public void LogPlayerThrowAttempt(int slotId)
    {
        if(!isMatchOver)
        {
            PlayerConfigurationSO[] pawns = GameManager.gameInstance.playerConfigurations;
            for (int i = 0; i < pawns.Length; i++)
            {
                if (pawns[i] != null && i == slotId - 1)
                {

                    pawns[i].matchStatAttempts++;
                }
            }
        }
      
    }

    public void LogPlayerKill(int slotId)
    {
        if(!isMatchOver)
        {
            PlayerConfigurationSO[] pawns = GameManager.gameInstance.playerConfigurations;
            for (int i = 0; i < pawns.Length; i++)
            {
                if (pawns[i] != null && i == slotId - 1)
                {
                    pawns[i].matchStatKills++;
                }
            }
        }
       
    }

    public void LogPlayerHit(int slotId)
    {
        if (!isMatchOver)
        {
            PlayerConfigurationSO[] pawns = GameManager.gameInstance.playerConfigurations;
            for (int i = 0; i < pawns.Length; i++)
            {
                if (pawns[i] != null && i == slotId - 1)
                {
                    pawns[i].matchStatHits++;
                }
            }
        }
        
    }

    public void LogPlayerCatch(int slotId)
    {
        if (!isMatchOver)
        {
            PlayerConfigurationSO[] pawns = GameManager.gameInstance.playerConfigurations;
            for (int i = 0; i < pawns.Length; i++)
            {
                if (pawns[i] != null && i == slotId - 1)
                {
                    pawns[i].matchStatCatches++;
                }
            }
        }

    }
    public void LogPlayerDodge(int slotId)
    {
        if (!isMatchOver)
        {
            PlayerConfigurationSO[] pawns = GameManager.gameInstance.playerConfigurations;
            for (int i = 0; i < pawns.Length; i++)
            {
                if (pawns[i] != null && i == slotId - 1)
                {
                    pawns[i].matchStatDodges++;
                }
            }
        }

    }

    public void ResetMatchStats()
    {
        isMatchOver = false;
        PlayerConfigurationSO[] pawns = GameManager.gameInstance.playerConfigurations;
        for (int i = 0; i < pawns.Length; i++)
        {
            if (pawns[i] != null)
            {
                pawns[i].matchStatKills = 0;
                pawns[i].matchStatHits = 0;
                pawns[i].matchStatCatches = 0;
                pawns[i].matchStatDodges = 0;
                pawns[i].matchStatAttempts = 0;
            }
        }
    }


}
