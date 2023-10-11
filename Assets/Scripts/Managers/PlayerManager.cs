using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] PlayerInput _playerInput;
    [SerializeField] PlayerStateMachine _playerStateMachine;
    [SerializeField] HealthHandler _healthHandler;

    public PlayerStateMachine PlayerStateMachine { get { return _playerStateMachine; } }
    public HealthHandler HealthHandler { get { return _healthHandler; } }   

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SetPlayerComponents();
    }
    private void OnEnable()
    {

    }
    private void OnDisable()
    {

    }

    public void Start()
    {
        
    }

    public void ActivatePlayer()
    {
        
        _playerStateMachine.enabled = true;
    }

    public void SetPlayerComponents()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerStateMachine = GetComponent<PlayerStateMachine>();
        _healthHandler = GetComponent<HealthHandler>();
    }

    public void Die()
    {
        /*_anim.SetBool("Die", true);
        Ctx.UnequipBall(Ctx.EquippedBall);
        Ctx.IsDead = true;*/
    }

}
