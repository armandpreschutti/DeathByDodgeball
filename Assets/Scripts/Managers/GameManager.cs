using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

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
    public Animator anim;

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
            StartCoroutine(StartMatch());
        }
    }
    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
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
        StartSceneTransition();
        yield return new WaitForSeconds(1);
        LoadScene("GamePlay");
    }


    public void InitializePlayers(Transform player1StartPoint, Transform player2StartPoint)
    {
        players[0].transform.position = player1StartPoint.position;
        players[1].transform.position = player2StartPoint.position;
        foreach (PlayerManager player in readyPlayers)
        {
            player.ActivatePlayer();
        }
    }

    public void StartSceneTransition()
    {
        anim.SetTrigger("Start");
    }
    public void EndSceneTransition()
    {
        anim.SetTrigger("End");
    }
}
