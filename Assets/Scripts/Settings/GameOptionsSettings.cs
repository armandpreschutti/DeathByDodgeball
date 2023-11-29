using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOptionsSettings : MonoBehaviour
{
    [SerializeField] Button _musicButton;
    [SerializeField] Button _sfxButton;
    [SerializeField] Button _returnToMenuButton;
    [SerializeField] Color _flashColor;
    [SerializeField] EventSystem _eventSystem;
    public bool musicActivated;
    public bool sfxActivated;
    [SerializeField] TextMeshProUGUI _musicText;
    [SerializeField] TextMeshProUGUI _sfxText;


    private void Start()
    {
        _eventSystem.firstSelectedGameObject = _musicButton.gameObject;
    }

    public void MusicButton()
    {
        Debug.Log("Music Toggled");
        musicActivated = !musicActivated;
        ToggleText(musicActivated, _musicText);
        GameManager.GetInstance().ToggleMusic(musicActivated);
    }

    public void SFXButton()
    {
        Debug.Log("SFX Toggled");
        sfxActivated= !sfxActivated;
        ToggleText(sfxActivated, _sfxText);
    }

    public void ReturnToMenuButton()
    {
        GameManager.GetInstance().SwitchScene("MainMenu");
    }

  /*  IEnumerator ButtonFlash(Button button)
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
*/
    public void ToggleText(bool value, TextMeshProUGUI text)
    {
        text.text = value ? "On" : "Off";
    }
}
