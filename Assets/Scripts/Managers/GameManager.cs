using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] PlayerInputManager _playerInputManager;
    [SerializeField] PreMatchManager _preMatchManager;
    [SerializeField] Animator _anim;
    [SerializeField] AudioSource _gameManagerAudio;
    public Color team1Color;
    public Color team2Color;
    public string sceneName;
    public PlayerInputManager PlayerInputManager { get { return _playerInputManager; } }
    public PreMatchManager PreMatchManager { get { return _preMatchManager; } set { _preMatchManager = value; } }

    public Animator Anim { get { return _anim; } }
    public AudioSource GameManagerAudio { get { return _gameManagerAudio; } }   

    public static event Action<PlayerInput> onPlayerFound;
    public static event Action onStartSceneTransition;
    public static event Action onEndSceneTransition;

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
        SceneManager.sceneLoaded += InitializeScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= InitializeScene;
    }

    public void Start()
    {

    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchScene("Gameplay");
        }
    }

    public void DisableJoining()
    {
        _playerInputManager.DisableJoining();
    }

    public void EnableJoining()
    {
        _playerInputManager.EnableJoining();
    }

    public void SetGameComponents()
    {
        _playerInputManager = GetComponent<PlayerInputManager>(); 
        _anim = GetComponentInChildren<Animator>();
        _gameManagerAudio = GetComponent<AudioSource>();
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        onPlayerFound?.Invoke(playerInput);
    }

    public void SwitchScene(string sceneName)
    {
        StartCoroutine(SwitchSceneCoroutine(sceneName));
    }

    public IEnumerator SwitchSceneCoroutine(string sceneName)
    {
        onStartSceneTransition?.Invoke();
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(sceneName);
    }

    public void InitializeScene(Scene scene, LoadSceneMode mode)
    {
        if(scene.name != "TitleMenu")
        {
            onEndSceneTransition?.Invoke();
        }
        else
        {
            return;
        }
        
    }

    public void CreatePreMatchInstance(Scene scene)
    {
        this.AddComponent<PreMatchManager>();
        _preMatchManager= GetComponent<PreMatchManager>();
    }
    
    public GameObject FindCurrentSettings()
    {
        // Define a regular expression pattern to match objects with "Settings" in their name.
        string pattern = ".*Settings.*";

        // Use FindObjectsOfType to find all GameObjects in the scene.
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        // Loop through all objects and find the first one that matches the pattern.
        foreach (GameObject obj in allObjects)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(obj.name, pattern))
            {
                return obj; // Return the first matching object.
            }
        }

        // If no matching object is found, return null.
        return null;
    }
}
