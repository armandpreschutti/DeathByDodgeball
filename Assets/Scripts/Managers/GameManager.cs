using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] PlayerInputManager _playerInputManager;
    [SerializeField] Animator _anim;
    [SerializeField] UIManager _uiManager;

    public PlayerInputManager PlayerInputManager { get; private set; }
    public Animator Anim { get; private set; }
    public UIManager UIManager { get; private set; }

    public event Action onTransitionStart;
    public event Action onTransitionEnd;

    public static GameManager gameInstance;

    public static GameManager GetInstance()
    {
        return gameInstance;
    }

    private void Awake()
    {
        if (gameInstance == null)
        {
            gameInstance = this;
            DontDestroyOnLoad(gameObject);
            SetGameComponents();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        onTransitionStart += StartSceneTransition;
        onTransitionEnd += EndSceneTransition;
    }

    private void OnDisable()
    {
        onTransitionStart -= StartSceneTransition;
        onTransitionEnd -= EndSceneTransition;
    }

    public void Start()
    {

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            onTransitionStart?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            onTransitionEnd?.Invoke();
        }
    }
    public void StartSceneTransition()
    {
        _anim.Play("Start");
    }
    public void EndSceneTransition()
    {
        _anim.Play("End");
    }
    public void SetGameComponents()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
        _anim = GetComponentInChildren<Animator>();   
        _uiManager = GetComponent<UIManager>(); 
    }
}
