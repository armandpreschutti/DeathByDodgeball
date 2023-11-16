using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuSettings : MonoBehaviour
{
    [SerializeField] Button _playButton;
    [SerializeField] Button _optionsButton;
    [SerializeField] Button _exitButton;
    [SerializeField] Color _flashColor;
    [SerializeField] EventSystem _eventSystem;
    private void Start()
    {
        _eventSystem.firstSelectedGameObject = _playButton.gameObject;
    }

    public void PlayButton()
    {
        Debug.LogWarning("Entering Match Configuration ...");
        StartCoroutine(ButtonFlash(_playButton));
        GameManager.GetInstance().SwitchScene("MatchConfiguration");
    }
    
    public void OptionsButton()
    {
        Debug.LogWarning("Entering Game Options ...");
        StartCoroutine(ButtonFlash(_optionsButton));
        GameManager.GetInstance().SwitchScene("GameOptions");
    }
    public void ExitButton()
    {
        StartCoroutine(ButtonFlash(_exitButton));
        Debug.LogWarning("Quitting game ...");
        Application.Quit();
    }
    IEnumerator ButtonFlash(Button button)
    {
        yield return new WaitForSeconds(.10f);
        button.image.color = _flashColor;
        yield return new WaitForSeconds(.10f);
        button.image.color = Color.white;
        yield return new WaitForSeconds(.10f);
        button.image.color = _flashColor;
        yield return new WaitForSeconds(.10f);
        button.image.color = Color.white;
    }
}
