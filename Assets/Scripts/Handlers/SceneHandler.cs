using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneHandler : MonoBehaviour
{
    public void LoadMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadPlayerSelection()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("PlayerSelection");
        
    }

    public void LoadGameInstance()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MatchInstance");
    }

    public void ExitGame()
    {
        Application.Quit();
    }    
}
