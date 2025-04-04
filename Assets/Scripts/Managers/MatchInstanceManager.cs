using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class MatchInstanceManager : MonoBehaviour
{
    public static MatchInstanceManager Instance { get; private set; }

    public PlayableDirector playableDirector;
    public TimelineAsset matchBeginCS;
    public TimelineAsset matchOverCS;
    public GameObject[] blueTeamConfigurations;
    public GameObject[] redTeamConfigurations;
    public GameObject[] blueActivePlayers;
    public GameObject[] redActivePlayers;
    public GameObject playablePawn;

    public static Action onInitializeMatchInstance;
    public static Action onStartMatch;
    public static Action onEndMatch;
    public static Action onDisablePawnControl;
    public static Action onEnablePawnControl;
    public static Action onStopMatchElements;
    public bool isMatchOver;

    public string matchWinner;

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        isMatchOver = false;
        playableDirector = GetComponent<PlayableDirector>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += ConfigureMatchInstance;
        PawnManager.onPlayerLoaded += AddPlayerToMatch;
        HealthSystem.onPlayerElimination += CheckMatchStatus;
        MatchTimer.onMatchTimeOut += MatchTimeOut;
        PauseMenuController.OnExitGame += ExitMatch;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= ConfigureMatchInstance;
        PawnManager.onPlayerLoaded -= AddPlayerToMatch;
        HealthSystem.onPlayerElimination -= CheckMatchStatus;
        MatchTimer.onMatchTimeOut -= MatchTimeOut;
        PauseMenuController.OnExitGame -= ExitMatch;
    }


    public void ConfigureMatchInstance(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MatchInstance" || scene.name == "Debug")
        {
            if(GameManager.gameInstance.playerConfigurations != null)
            {
                for (int i = 0; i < GameManager.gameInstance.playerConfigurations.Length; i++)
                {
                    if (GameManager.gameInstance.playerConfigurations[i] != null)
                    {
                        CreatePlayerInstance(GameManager.gameInstance.playerConfigurations[i]);
                    }
                }
            }

            matchWinner = "Draw";
            onInitializeMatchInstance?.Invoke();
            if (!GameManager.gameInstance.isDebugging)
            {
                PlayCutScene(matchBeginCS);
            }
        }
        if(scene.name == "MainMenu")
        {
            Destroy(gameObject);
        }
    }

    public void CreatePlayerInstance(PlayerConfigurationSO so)
    {
        GameObject pawn = playablePawn;
        PawnManager pawnManager = pawn.GetComponent<PawnManager>();
        pawn.name = $"PlayablePawn{so.playerId}";
        pawnManager.slotId = so.slotId;
        pawnManager.skinId = so.skinID;
        pawnManager.playerId = so.playerId;
        pawnManager.teamId = so.teamId;
        pawnManager.pawnColor = so.playerColor;
        Instantiate(pawn, transform.position, Quaternion.identity);
    }

    public void AddPlayerToMatch(int slotId, int pId, GameObject pawn)
    {
        switch (slotId)
        {
            case 1:
                blueTeamConfigurations[0] = pawn;
                blueActivePlayers[0] = pawn;
                break;
            case 2:
                blueTeamConfigurations[1] = pawn;
                blueActivePlayers[1] = pawn;
                break;
            case 3:
                redTeamConfigurations[0] = pawn;
                redActivePlayers[0] = pawn;
                break;
            case 4:
                redTeamConfigurations[1] = pawn;
                redActivePlayers[1] = pawn;
                break;
        }
    }

    public void StartMatch()
    {
        onStartMatch?.Invoke();
    }

    public void EnablePawnControl()
    {
        onEnablePawnControl?.Invoke();
    }

    public void DisablePawnControl()
    {
        onDisablePawnControl?.Invoke();
    }

    public void EndMatch()
    {
        onEndMatch?.Invoke();
        SceneManager.LoadScene("PostMatch");
    }

    public void CheckMatchStatus(int slotId)
    {
        switch (slotId)
        {
            case 1:
                blueActivePlayers[0] = null;
                break;
            case 2:
                blueActivePlayers[1] = null;
                break;
            case 3:
                redActivePlayers[0] = null;
                break;
            case 4:
                redActivePlayers[1] = null;
                break;
        }

        // Check if the blue team is fully empty
        bool isBlueTeamEmpty = true;
        for (int i = 0; i < blueActivePlayers.Length; i++)
        {
            if (blueActivePlayers[i] != null)
            {
                isBlueTeamEmpty = false;
                break;
            }
        }

        // Check if the red team is fully empty
        bool isRedTeamEmpty = true;
        for (int i = 0; i < redActivePlayers.Length; i++)
        {
            if (redActivePlayers[i] != null)
            {
                isRedTeamEmpty = false;
                break;
            }
        }

        // Log which team is empty
        if (isBlueTeamEmpty)
        {
            isMatchOver = true;
            onStopMatchElements?.Invoke();
            matchWinner = "Red";
            GameManager.gameInstance.winningTeam = "Red";
            PlayCutScene(matchOverCS);
        }

        if (isRedTeamEmpty)
        {
            isMatchOver = true;
            onStopMatchElements?.Invoke();
            matchWinner = "Blue";
            GameManager.gameInstance.winningTeam = "Blue";
            PlayCutScene(matchOverCS);
        }
    }

    public void PlayCutScene(PlayableAsset asset)
    {
        playableDirector.playableAsset = asset;
        playableDirector.initialTime = 0.0f;
        playableDirector.Play();
    }
    
    public void MatchTimeOut()
    {
        isMatchOver = true;

        // Check if the blue team is fully empty
        int blueTeamKills = 0;
        int redTeamKills = 0;
        int blueTeamHits = 0;
        int redTeamHits = 0;
        int blueTeamCatches = 0;
        int redTeamCatches = 0;
        PlayerConfigurationSO[] blueTeam = new PlayerConfigurationSO[2];
        PlayerConfigurationSO[] redTeam = new PlayerConfigurationSO[2];

        int blueTeamCount = 0;
        int redTeamCount = 0;

        for (int i = 0; i < GameManager.gameInstance.playerConfigurations.Length; i++)
        {
            if (GameManager.gameInstance.playerConfigurations[i] != null)
            {
                if (GameManager.gameInstance.playerConfigurations[i].teamId == 1)
                {
                    blueTeam[blueTeamCount] = GameManager.gameInstance.playerConfigurations[i];
                    blueTeamCount++;
                }
                else
                {
                    redTeam[redTeamCount] = GameManager.gameInstance.playerConfigurations[i];
                    redTeamCount++;
                }
            }
        }



        for (int i = 0; i < blueTeam.Length; i++)
        {
            if (blueTeam[i] != null)
            {
                blueTeamKills += blueTeam[i].matchStatKills;
                blueTeamHits += blueTeam[i].matchStatHits;
                blueTeamCatches += blueTeam[i].matchStatCatches;
            }
        }

        for (int i = 0; i < redTeam.Length; i++)
        {
            if (redTeam[i] != null)
            {
                redTeamKills += redTeam[i].matchStatKills;
                redTeamHits += redTeam[i].matchStatHits;
                redTeamCatches += redTeam[i].matchStatCatches;
            }
        }

        if (blueTeamKills > redTeamKills)
        {
            matchWinner = "Blue";
            GameManager.gameInstance.winningTeam = "Blue";
        }
        else if(blueTeamKills < redTeamKills)
        {
            matchWinner = "Red";
            GameManager.gameInstance.winningTeam = "Red";
        }
        else
        {
            if (blueTeamHits > redTeamHits)
            {
                matchWinner = "Blue";
                GameManager.gameInstance.winningTeam = "Blue";
            }
            else if (blueTeamHits < redTeamHits)
            {
                matchWinner = "Red";
                GameManager.gameInstance.winningTeam = "Red";
            }
            else
            {
                if (blueTeamCatches > redTeamCatches)
                {
                    matchWinner = "Blue";
                    GameManager.gameInstance.winningTeam = "Blue";
                }
                else if (blueTeamCatches < redTeamCatches)
                {
                    matchWinner = "Red";
                    GameManager.gameInstance.winningTeam = "Red";
                }
                else
                {
                    matchWinner = "Draw";
                    GameManager.gameInstance.winningTeam = "Draw";
                }
            }
        }

        PlayCutScene(matchOverCS);

    }
    public void ExitMatch()
    {
        isMatchOver = true;
        onStopMatchElements?.Invoke();
        matchWinner = "Draw";
        GameManager.gameInstance.winningTeam = "Draw";
        PlayCutScene(matchOverCS);
    }

}
