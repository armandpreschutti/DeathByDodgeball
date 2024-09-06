using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneHandler : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadPlayerSelection()
    {
        SceneManager.LoadScene("PlayerSelection");
        
    }

    public void LoadGameInstance()
    {
        SceneManager.LoadScene("MatchInstance");
    }

    public void ExitGame()
    {
        Application.Quit();
    }    
}
