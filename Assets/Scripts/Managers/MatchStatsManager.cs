using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchStatsManager : MonoBehaviour
{
    private void OnEnable()
    {
        BallManager.onPlayerHit += LogPlayerHit;
        BallManager.onPlayerKill += LogPlayerKill;
        BallManager.onPlayerThrowAttempt += LogPlayerThrowAttempt;
        MatchInstanceManager.onStartMatch += ResetMatchStats;
        BallManager.onCaught += LogPlayerCatch;
        PlayerStateMachine.onDodged += LogPlayerDodge;
    }
    private void OnDisable()
    {
        BallManager.onPlayerHit -= LogPlayerHit;
        BallManager.onPlayerKill -= LogPlayerKill;
        BallManager.onPlayerThrowAttempt -= LogPlayerThrowAttempt;
        MatchInstanceManager.onStartMatch -= ResetMatchStats;
        BallManager.onCaught -= LogPlayerCatch;
        PlayerStateMachine.onDodged -= LogPlayerDodge;
    }

    public void LogPlayerThrowAttempt(int slotId)
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

    public void LogPlayerKill(int slotId)
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

    public void LogPlayerHit(int slotId)
    {
        PlayerConfigurationSO[] pawns = GameManager.gameInstance.playerConfigurations;
        for (int i = 0; i < pawns.Length; i++)
        {
            if (pawns[i] != null && i == slotId-1)
            {
                pawns[i].matchStatHits++;
            }
        }
    }

    public void LogPlayerCatch(int slotId)
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
    public void LogPlayerDodge(int slotId)
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

    public void ResetMatchStats()
    {
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
