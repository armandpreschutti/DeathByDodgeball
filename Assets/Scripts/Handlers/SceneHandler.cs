using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
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

    public void ExitGame()
    {
        Application.Quit();
    }

    
}
