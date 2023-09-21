using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] InputAction equipAction;
    [SerializeField] InputAction readyAction;
    [SerializeField] Canvas playerCanvas;
    public enum InventoryState { unequipped, equipped }
    public InventoryState currentInventoryState = InventoryState.unequipped;
    public GameObject equippedBall = null;
    public Transform holdPosition;
    public bool canUnequip;
    public bool isReady;
    public Transform target;

    [Header("Components")]
    public Collider2D col;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Animator anim;
    public LocomotionHandler locomotionHandler;
    public MeleeHandler meleeHandler;
    public HealthHandler healthHandler;
    public GameObject throwPowerBar;
    public Slider dodgeSlider;
    public Slider healthSlider;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        readyAction = playerInput.actions["Ready"];
    }

    private void OnEnable()
    {
        readyAction.performed += Ready;
    }

    public void Start()
    {
        InitializePlayer();
    }

    public void InitializePlayer()
    {
        if (playerInput != null)
        {
            // Set the GameObject's name based on the player index
            GameManager.GetInstance().AddPlayer(this);
            name = $"Player{playerInput.playerIndex + 1}";
            DontDestroyOnLoad(gameObject);
            GameObject.Find($"{name}Prompt").GetComponent<TextMeshProUGUI>().text = "Press LB or F when ready";
        }
    }

    public void Ready(InputAction.CallbackContext context)
    {
        if (!isReady)
        {
            isReady= true;
            GameManager.GetInstance().PlayerReady(this);
            GameObject.Find($"{name}Prompt").GetComponent<TextMeshProUGUI>().text = "Ready!";
        }
        else
        {
            return;
        }
    }

    public void EquipBall(GameObject ball)
    {
        equippedBall= ball;
        currentInventoryState= InventoryState.equipped;
        equippedBall.GetComponent<Collider2D>().enabled= false;
        equippedBall.GetComponent<Rigidbody2D>().simulated= false;
        equippedBall.transform.localPosition = holdPosition.localPosition;
    }

    public void ActivateBall()
    {
        currentInventoryState = InventoryState.unequipped;
        equippedBall = null;
    }
    public void ActivatePlayer()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        locomotionHandler = GetComponent<LocomotionHandler>();
        meleeHandler = GetComponent<MeleeHandler>();
        healthHandler = GetComponent<HealthHandler>();

        locomotionHandler.playerManager = this;
        meleeHandler.playerManager = this;
        healthHandler.playerManager = this;
        dodgeSlider = GameObject.Find($"{name}DodgeBar").GetComponent<Slider>();
        healthSlider = GameObject.Find($"{name}HealthBar").GetComponent<Slider>();
        switch (name)
        {
            case "Player1":
                target = GameObject.Find("Player2").transform;
                break;
            case "Player2":
                target = GameObject.Find("Player1").transform;
                break;
            default:
                break;
        }

        col.enabled = true;
        rb.simulated = true;
        spriteRenderer.enabled = true;  
        anim.enabled = true;
        locomotionHandler.enabled = true;
        meleeHandler.enabled = true;
        healthHandler.enabled = true;
    }
    
}
