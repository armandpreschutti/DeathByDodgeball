using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PostMatchManager : MonoBehaviour
{
    public MatchInstanceManager matchInstanceManager;
    public TextMeshProUGUI winnerText;

    private void Awake()
    {
        matchInstanceManager = FindAnyObjectByType<MatchInstanceManager>();
    }
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }

    private void Start()
    {
        winnerText.text = $"{matchInstanceManager.matchWinner} team wins!";
        switch (matchInstanceManager.matchWinner)
        {
            case "Red":
                winnerText.text = $"{matchInstanceManager.matchWinner} team wins!";
                winnerText.color = Color.red;
                break;
            case "Blue":
                winnerText.text = $"{matchInstanceManager.matchWinner} team wins!";
                winnerText.color = Color.blue;
                break;
            default:
                winnerText.text = $"No contest!";
                winnerText.color = Color.white;
                break;
        }
        Destroy(matchInstanceManager.gameObject);
    }
}
