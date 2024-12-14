using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuController : MonoBehaviour
{
    public bool isPaused;
    [SerializeField] GameObject _pauseMenu;
    public GameObject firstSelected;
    public GameObject ControllerMap;
    public GameObject Buttons;
    public EventSystem eventSystem;

    public static Action<bool> OnGamePaused;
    public static Action OnExitGame;

    private void Awake()
    {
        eventSystem = FindObjectOfType<EventSystem>();
    }

    private void OnEnable()
    {
        UserController.onPausePressed += SetPauseMenuState;
    }

    private void OnDisable()
    {
        UserController.onPausePressed -= SetPauseMenuState;
    }

    public void SetPauseMenuState()
    {
        _pauseMenu.SetActive(!_pauseMenu.activeSelf);
        OnGamePaused?.Invoke(_pauseMenu.activeSelf);
        isPaused = _pauseMenu.activeSelf;
        eventSystem.SetSelectedGameObject(firstSelected);
        if (_pauseMenu.activeSelf == false)
        {
            Buttons.SetActive(true);
            ControllerMap.SetActive(false);
        }
    }
    public void ExitGame()
    {
        OnExitGame?.Invoke();
        OnGamePaused?.Invoke(false);
        Destroy(gameObject);
    }

}
