using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PostMatchManager : MonoBehaviour
{
    public TextMeshProUGUI winnerText;
    public PlayerConfigurationSO[] pawns;
    public GameObject PawnPanel;
    public RectTransform panel1Transform;
    public RectTransform panel2Transform;
    public RectTransform panel3Transform;
    public RectTransform panel4Transform;


    private void Awake()
    {
        pawns = GameManager.gameInstance.playerConfigurations;
        for(int i = 0; i < pawns.Length; i++)
        {
            if (pawns[i] != null && pawns[i].slotId != 0)
            {
                GameObject pawnPanel;
                pawnPanel = Instantiate(PawnPanel, pawnPanelPosition(pawns[i]));
                pawnPanel.GetComponent<PawnPostMatchObserver>().pawnConfig = pawns[i];
            }
        }
    }

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

    public RectTransform pawnPanelPosition(PlayerConfigurationSO pawn)
    {
        RectTransform returnPanel;
        switch (pawn.slotId)
        {
            case 1:
                returnPanel = panel1Transform;
                break;
            case 2:
                returnPanel = panel2Transform;
                break;
            case 3:
                returnPanel = panel3Transform;
                break;
            case 4:
                returnPanel = panel4Transform;
                break;
            default:
                return null;
        }
        return returnPanel;
    }
}
