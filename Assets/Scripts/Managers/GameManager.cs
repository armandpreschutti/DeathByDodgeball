using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager gameInstance;
    public PlayerInputManager playerInputManager;
    public int playerCount = 0;
    public int playerRedayCount = 0;
    public int nextScene = 0;
    public int pregameCount;
    public List<PlayerManager> players; 
    public List<PlayerManager> readyPlayers;
    public GameObject gameModes;
    

    /// <summary>
    /// When called, this function returns the current singleton instance of the game manager
    /// </summary>
    /// <returns>game manager instance</returns>
    public static GameManager GetInstance()
    {
        return gameInstance;
    }

    private void Awake()
    {
        if (gameInstance == null)
        {
            gameInstance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

  
    public void AddPlayer(PlayerManager player)
    {
        players.Add(player);
        
    }
    public void PlayerReady(PlayerManager player)
    {
        readyPlayers.Add(player);
        if(readyPlayers.Count == playerInputManager.maxPlayerCount && readyPlayers.All(player => player.isReady))
        {
            // Start the countdown coroutine
            StartCoroutine(StartMatch());
        }
    }
    public void NextScene()
    {
        SceneManager.LoadScene(nextScene);
    }

    private IEnumerator StartMatch()
    {
        float countdown = pregameCount;
        GameObject.Find("StartPrompt").GetComponent<TextMeshProUGUI>().text = "Match Starting!";

        while (countdown > 0f)
        {
            GameObject.Find("Count").GetComponent<TextMeshProUGUI>().text = countdown.ToString();
            countdown -= 1f;
            yield return new WaitForSeconds(1f);
        }

        NextScene();
    }


    public void InitializePlayers(Transform player1StartPoint, Transform player2StartPoint)
    {
        players[0].transform.position = player1StartPoint.position;
        players[1].transform.position = player2StartPoint.position;
        foreach(PlayerManager player in readyPlayers)
        {
            player.ActivatePlayer();
        }
    }
    
}
