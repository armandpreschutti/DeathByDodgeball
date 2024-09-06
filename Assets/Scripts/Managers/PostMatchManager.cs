using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PostMatchManager : MonoBehaviour
{
    public TextMeshProUGUI winnerText;

    private void Start()
    {
        string winner = GameManager.gameInstance.winningTeam;
        winnerText.text = $"{winner} team wins!";
        switch (winner)
        {
            case "Red":
                winnerText.text = $"{winner} team wins!";
                winnerText.color = Color.red;
                break;
            case "Blue":
                winnerText.text = $"{winner} team wins!";
                winnerText.color = Color.blue;
                break;
            default:
                winnerText.text = $"No contest!";
                winnerText.color = Color.white;
                break;
        }
    }
}
