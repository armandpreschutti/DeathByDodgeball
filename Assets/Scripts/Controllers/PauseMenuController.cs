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
    public static Action<bool> OnReturnToMenu;

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
        ResetPauseMenu();
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

    public void ReturnToMenu()
    {
        OnReturnToMenu?.Invoke(true);
    }   

    // Set input values
    private void ResetPauseMenu()
    {
        // Set up input actions for player controls
        //_pauseControls.Player.Pause.performed -= ctx => SetPauseMenuState(/*ctx.ReadValue<Vector2>()*/);
        _pauseMenu.SetActive(!_pauseMenu.activeSelf);
        OnGamePaused?.Invoke(_pauseMenu.activeSelf);
        isPaused = _pauseMenu.activeSelf;
        if (isPaused)
        {
            eventSystem.SetSelectedGameObject(firstSelected);
        }
        //  _pauseControls.Player.Navigate.performed -= ctx => SetPauseNavigationInput();
    }
}
