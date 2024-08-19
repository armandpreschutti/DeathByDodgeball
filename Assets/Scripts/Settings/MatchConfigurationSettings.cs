using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchConfigurationSettings : MonoBehaviour
{
    [SerializeField] GameObject _exitPrompt;
    [SerializeField] GameObject _startMatchPrompt;
    [SerializeField] EventSystem _eventSystem;
    [SerializeField] Button _exitButton;
    [SerializeField] Button _returnButton;
    [SerializeField] Button _startMatchButton;

    public static event Action onReturnToMenu;

    private void OnEnable()
    {
        GameManagerUIObserver.onSceneTransitionEnd += GameManager_Depricated.GetInstance().CreatePreMatchInstance;
    }

    private void OnDisable()
    {
        GameManagerUIObserver.onSceneTransitionEnd -= GameManager_Depricated.GetInstance().CreatePreMatchInstance;

    }
    private void Start()
    {
        GameManager_Depricated.GetInstance().DisableJoining();
    }
    public void EnterLocalGameplaySession()
    {
        GameManager_Depricated.GetInstance().PreMatchManager.CreateLocalMatchInstance();
        GameManager_Depricated.GetInstance().SwitchScene("Gameplay");
    }
    public void ReturnToMainMenu()
    {
        onReturnToMenu?.Invoke();
        GameManager_Depricated.GetInstance().SwitchScene("MainMenu");
    }
}
