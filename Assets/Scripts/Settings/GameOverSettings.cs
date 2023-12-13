using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameOverSettings : MonoBehaviour
{
    [SerializeField] Button _replayButton;
    [SerializeField] Button _returnToMenu;
    [SerializeField] EventSystem _eventSystem;
    [SerializeField] TextMeshProUGUI winPrompt;
    [SerializeField] GameObject blueSprites;
    [SerializeField] GameObject redSprites;

    public void Start()
    {
        _eventSystem.firstSelectedGameObject = _replayButton.gameObject;
        foreach (GameObject player in GameManager.GetInstance().GetComponent<LocalMatchManager>().currentPlayers)
        {
            player.GetComponent<PlayerManager>().HidePlayer(true);
        }
        DisplayWinnerText();
    }

    public void ReturnToMenu()
    {
        GameManager.GetInstance().SwitchScene("MainMenu");
        foreach(GameObject player in GameManager.GetInstance().GetComponent<LocalMatchManager>().currentPlayers)
        {
            Destroy(player.gameObject);
        }
        Destroy(GameManager.GetInstance().GetComponent<LocalMatchManager>());
        Destroy(GameManager.GetInstance().GetComponent<PreMatchManager>());
    }

    public void ReplayGame()
    {
        GameManager.GetInstance().SwitchScene("Gameplay");
    }
    public void DisplayWinnerText()
    {
       /* winPrompt.text = $"Team {GameManager.GetInstance().GetComponent<LocalMatchManager>().winningTeam} wins!";*/
        if(GameManager.GetInstance().GetComponent<LocalMatchManager>().winningTeam == 1)
        {
            winPrompt.text = "Blue Team Wins!";
            winPrompt.color = Color.blue;
            blueSprites.SetActive(true);

        }
        else if (GameManager.GetInstance().GetComponent<LocalMatchManager>().winningTeam == 2)
        {
            winPrompt.text = "Red Team Wins!";
            winPrompt.color = Color.red;
            redSprites.SetActive(true);
        }
        else
        {
            return;
        }
    }
}
