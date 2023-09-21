using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class GameplaySettings : MonoBehaviour
{
    public Transform player1StartPoint;
    public Transform player2StartPoint;
    public GameObject gameOverPanel;
    public TextMeshProUGUI winText;

    private void Start()
    {
        GameManager.GetInstance().InitializePlayers(player1StartPoint, player2StartPoint);
    }
    public void GameOver(string winnerText)
    {
        gameOverPanel.SetActive(true);
        winText.text = winnerText;
    }
    public void ReplayGame()
    {
        SceneManager.LoadScene(1);
    }
}
