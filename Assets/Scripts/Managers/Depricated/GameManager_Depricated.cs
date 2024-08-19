using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager_Depricated : MonoBehaviour
{

    [SerializeField] PlayerInputManager _playerInputManager;
    [SerializeField] PreMatchManager _preMatchManager;
    [SerializeField] Animator _anim;
    [SerializeField] AudioSource _gameManagerAudio;
    [SerializeField] GameObject _musicManager;
    [SerializeField] GameObject _pauseBackground;
    public Color team1Color;
    public Color team2Color;
    public string sceneName;
    public bool musicEnabled;
    public bool sfxEnabled;
    public bool gamePaused;
    public PlayerInputManager PlayerInputManager { get { return _playerInputManager; } }
    public PreMatchManager PreMatchManager { get { return _preMatchManager; } set { _preMatchManager = value; } }
    public Animator Anim { get { return _anim; } }
    public AudioSource GameManagerAudio { get { return _gameManagerAudio; } }   
    public GameObject MusicManager {get { return _musicManager; } }

    
    public static event Action onStartSceneTransition;
    public static event Action onEndSceneTransition;

    public static GameManager_Depricated gameInstance;

    public static GameManager_Depricated GetInstance()
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
        PlayerManager_Depricated.OnPlayerPause += PauseGame;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= InitializeScene;
        PlayerManager_Depricated.OnPlayerPause += PauseGame;
    }

    public void Start()
    {

    }

    public void Update()
    {
        if(Input.GetKey(KeyCode.X))
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                SwitchScene("TitleMenu");
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                SwitchScene("MainMenu");
            }
            if (Input.GetKeyDown(KeyCode.F3))
            {
                SwitchScene("OptionsMenu");
            }
            if (Input.GetKeyDown(KeyCode.F4))
            {
                SwitchScene("MatchConfiguration");
            }
            if (Input.GetKeyDown(KeyCode.F5))
            {
                SwitchScene("Gameplay");
            }
            if (Input.GetKeyDown(KeyCode.F11))
            {
                Screen.fullScreen = !Screen.fullScreen;
            }
            if (Input.GetKeyDown(KeyCode.F12))
            {
                Application.Quit();
            }
        }
        else
        {
            return;
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

    public void EnablePlayerControllers()
    {
        // Get all the PlayerInput instances in the scene
        PlayerInput[] playerInputs = FindObjectsOfType<PlayerInput>();

        // Disable all of them
        foreach (PlayerInput playerInput in playerInputs)
        {
            playerInput.enabled = true;
        }
    }

    public void DisablePlayerControllers()
    {
        // Get all the PlayerInput instances in the scene
        PlayerInput[] playerInputs = FindObjectsOfType<PlayerInput>();

        // Disable all of them
        foreach (PlayerInput playerInput in playerInputs)
        {
            playerInput.enabled = false;
        }
    }

    public void ToggleMusic(bool value)
    {
        MusicManager.SetActive(value);
    }

    public void PauseGame()
    {
        if (SceneManager.GetActiveScene().name == "Gameplay")
        {
            gamePaused = !gamePaused;
            foreach (GameObject player in GetComponent<LocalMatchManager>().currentPlayers)
            {
                player.GetComponent<Animator>().speed = gamePaused ? 0f : 1f;
                player.GetComponent<PlayerStateMachine>().enabled = gamePaused ? false : true;
            }
            Time.timeScale = gamePaused ? 0f : 1f;
            if (gamePaused)
            {
                _pauseBackground.SetActive(true);
                MusicManager.GetComponent<AudioSource>().Pause();
            }
            else
            {
                _pauseBackground.SetActive(false);
                MusicManager.GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            return;
        }
       
    }
}
