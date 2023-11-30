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
        winPrompt.text = $"Team {GameManager.GetInstance().GetComponent<LocalMatchManager>().winningTeam} wins!";
    }
}
