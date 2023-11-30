using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using System.Globalization;

public class LocalMatchManager : MonoBehaviour
{
    public List<GameObject> currentPlayers;
    public List<GameObject> team1;
    public List<GameObject> team2;
    public List<GameObject> team1Alive;
    public List<GameObject> team2Alive;
    public GameObject winPrompt;
    public static event Action onMatchCountdown;
    public static event Action onMatchStart;
    public static event Action onActivatePlayers;
    public static event Action onDeactivatePlayers;
    public static event Action onResetPlayers;
    public int winningTeam;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += InitializeScene;
        GameManagerUIObserver.onSceneTransitionEnd += InitializeMatch;
        GameManagerUIObserver.onMatchCountdownEnd += BeginMatch;
        PlayerManager.OnPlayerDeath += ElminiatePlayer;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= InitializeScene;
        GameManagerUIObserver.onSceneTransitionEnd -= InitializeMatch;
        GameManagerUIObserver.onMatchCountdownEnd -= BeginMatch;
        PlayerManager.OnPlayerDeath -= ElminiatePlayer;
    }
  
    public void InitializeScene(Scene scene, LoadSceneMode mode)
    {
        foreach (GameObject player in currentPlayers)
        {
            SpriteRenderer rend = player.GetComponent<SpriteRenderer>();
            rend.enabled = true;
        }
        onDeactivatePlayers?.Invoke();
        onResetPlayers?.Invoke();
    }
    public void InitializeMatch(Scene scene)
    {
        if(scene.name == "Gameplay")
        {
            InitializeTeams();
            onMatchCountdown?.Invoke();
        }
        else
        {
            return;
        }        
    }

    public void InitializeTeams()
    {
        team1Alive = new List<GameObject>();
        team2Alive = new List<GameObject>();

        foreach (GameObject player in team1)
        {
            team1Alive.Add(player);
            
        }
        foreach (GameObject player in team2)
        {
            team2Alive.Add(player);
        }
    }

    public void TriggerMatchStart()
    {
        onMatchStart?.Invoke();
    }
    public void BeginMatch()
    {
        onActivatePlayers?.Invoke();
    }

    public void ElminiatePlayer(PlayerManager playerManager)
    {
        Debug.Log($"{playerManager.name} is recognized as dead");
        if(playerManager.TeamId == 1)
        {
            team1Alive.Remove(playerManager.gameObject);
        }
        else if (playerManager.TeamId == 2)
        {
            team2Alive.Remove(playerManager.gameObject);
        }
        else
        {
            Debug.LogError("This most recent player death didn't have a team");
            return;
        }
        CheckForWinner();
    }

    public void CheckForWinner()
    {
        winPrompt = GameObject.Find("WinPrompt");
        winPrompt.GetComponent<TextMeshProUGUI>().text = "Game!";
        if (team1Alive.Count <= 0) 
        {
            winningTeam = 2;
        }
        else if (team2Alive.Count <= 0)
        {
            winningTeam = 1;
        }
        else
        {
            return;
        }
        StartCoroutine(ContinueToPostGame());
    }
    public IEnumerator ContinueToPostGame()
    {
        yield return new WaitForSeconds(2f);
        GameManager.GetInstance().SwitchScene("PostGameplay");
    }

}
