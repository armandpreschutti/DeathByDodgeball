using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostMatchManager : MonoBehaviour
{
    public TextMeshProUGUI winnerText;
    public PlayerConfigurationSO[] pawns;
    public GameObject PawnPanel;
    public RectTransform panel1Transform;
    public RectTransform panel2Transform;
    public RectTransform panel3Transform;
    public RectTransform panel4Transform;
    public Image winnerBackground;
    public Sprite blueBackground;
    public Sprite redBackground;
    public Sprite yellowBackground;



    private void Awake()
    {
        pawns = GameManager.gameInstance.playerConfigurations;
        for(int i = 0; i < pawns.Length; i++)
        {
            if (pawns[i] != null && pawns[i].slotId != 0)
            {
                GameObject pawnPanel;
                pawnPanelPosition(pawns[i]).gameObject.SetActive(true);
                pawnPanel = Instantiate(PawnPanel, pawnPanelPosition(pawns[i]));
                pawnPanel.GetComponent<PawnPostMatchObserver>().pawnConfig = pawns[i];

            }
        }
    }

    private void Start()
    {
        string winner = GameManager.gameInstance.winningTeam;
        //winnerText.text = $"{winner} team wins!";
        switch (winner)
        {
            case "Red":
                winnerText.text = $"{winner} team wins!";
                winnerBackground.sprite = redBackground;
                break;
            case "Blue":
                winnerText.text = $"{winner} team wins!";
                winnerBackground.sprite = blueBackground ;
                break;
            case "Draw":
                winnerText.text = "Draw!";
                winnerBackground.sprite = yellowBackground;
                break;
            default:
                winnerText.text = $"No contest!";
                winnerBackground.sprite = yellowBackground;
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
