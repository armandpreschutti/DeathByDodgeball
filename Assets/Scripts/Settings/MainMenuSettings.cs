using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
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
        GameManager.GetInstance().DisableJoining();
    }

    public void PlayButton()
    {
        StartCoroutine(ButtonFlash(_playButton));
        GameManager.GetInstance().SwitchScene("MatchConfiguration");
    }
    
    public void OptionsButton()
    {
        StartCoroutine(ButtonFlash(_optionsButton));
        GameManager.GetInstance().SwitchScene("GameOptions");
    }

    public void ExitButton()
    {
        StartCoroutine(ButtonFlash(_exitButton));
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
