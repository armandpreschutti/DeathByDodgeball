using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PawnPostMatchObserver : MonoBehaviour
{
    public PlayerConfigurationSO pawnConfig;
    public Image pawnPreview;
    public TextMeshProUGUI pawnTag;
    public Image pawnPanel;
    public PreviewSkinsSO blueSkins;
    public PreviewSkinsSO redSkins;
    public TextMeshProUGUI hitsText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI catchesText;
    public TextMeshProUGUI dodgesText;
    public TextMeshProUGUI accuracyText;
    public Sprite winSprite;
    public Sprite loseSprite;
    public Image pawnBg;


    // Start is called before the first frame update
    void Start()
    {
        SetPawnPreview();
        SetPawnTag();
        SetMatchStats();
        SetPawnBackground();
    }

    public void SetPawnPreview()
    {
        switch(pawnConfig.teamId)
        {
            case 1:
                pawnPreview.sprite = blueSkins.skins[pawnConfig.skinID];
                break;

            case 2:
                pawnPreview.sprite = redSkins.skins[pawnConfig.skinID];
                pawnPreview.rectTransform.localScale = new Vector3(-1,1,1); 
                break;
        }
    }

    public void SetPawnTag()
    {

        if(pawnConfig.playerId != 0)
        {
            pawnTag.text = $"P{pawnConfig.playerId}";
        }
        else
        {
            pawnTag.text = "CPU";
        }
    }
    public void SetPawnBackground()
    {
        string winner = GameManager.gameInstance.winningTeam;
        switch(winner)
        {
            case "Blue":
                if(pawnConfig.teamId == 1)
                {
                    pawnBg.sprite = winSprite;
                }
                else
                {
                    pawnBg.sprite = loseSprite;
                }
                break;
            case "Red":
                if (pawnConfig.teamId == 2)
                {
                    pawnBg.sprite = winSprite;
                }
                else
                {
                    pawnBg.sprite = loseSprite;
                }
                break;
            case "Draw":
                pawnBg.sprite = loseSprite;
                break;
            default:
                Debug.LogError("Could not find winner in gamemanager");
                break;
        }

    }

    public void SetMatchStats()
    {
        killsText.text = pawnConfig.matchStatKills.ToString();
        hitsText.text = pawnConfig.matchStatHits.ToString();
        catchesText.text = pawnConfig.matchStatCatches.ToString();
        dodgesText.text = pawnConfig.matchStatDodges.ToString();
        accuracyText.text = $"{throwAccuracy(pawnConfig.matchStatAttempts, pawnConfig.matchStatHits)}%";
    }

    public float throwAccuracy(int attempts, int hits)
    {
        float accuracy;
        if(attempts != 0)
        {
            accuracy = (hits * 100) / attempts;
        }
        else
        {
            accuracy = 0;
        }

        return accuracy;
    }

}
