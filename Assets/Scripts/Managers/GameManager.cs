using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
/*    [SerializeField] PlayerInputManager _playerInputManager;
    [SerializeField] PreMatchManager _preMatchManager;
    public PlayerInputManager PlayerInputManager { get { return _playerInputManager; } }*/

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
    public void Start()
    {

    }

    public void Update()
    {
      

    }
    public void SetGameComponents()
    {
        //_playerInputManager = GetComponent<PlayerInputManager>();
    }

  
 
}
