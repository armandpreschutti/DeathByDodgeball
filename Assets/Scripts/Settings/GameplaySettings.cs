using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplaySettings : MonoBehaviour
{
    public AudioClip music;
    public Transform player1StartPoint;
    public Transform player2StartPoint;
    public float postGameTime;

    private void Start()
    {
       
    }
    public void GameOver()
    {
    }
    public void ReplayGame()
    {
        SceneManager.LoadScene(1);
    }
 
}
