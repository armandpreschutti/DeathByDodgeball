using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class GameplaySettings : MonoBehaviour
{
    public AudioClip music;
    public Transform player1StartPoint;
    public Transform player2StartPoint;
    public float postGameTime;


    private void Start()
    {
        StartCoroutine(StartGame());
    }
    public void GameOver()
    {
        StartCoroutine(EndGame());
    }
    public void ReplayGame()
    {
        SceneManager.LoadScene(1);
    }
    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(3);
        GameManager.GetInstance().StartSceneTransition();
        yield return new WaitForSeconds(1);
        GameManager.GetInstance().LoadScene("GameOver");
    }
    IEnumerator StartGame()
    {
        GameManager.GetInstance().InitializePlayers(player1StartPoint, player2StartPoint);
       // GameManager.GetInstance().GetComponent<AudioSource>().clip = music;
        GameManager.GetInstance().EndSceneTransition();
        yield return new WaitForSeconds(.5f);
        //GameManager.GetInstance().GetComponent<AudioSource>().Play();
    }
   
}
