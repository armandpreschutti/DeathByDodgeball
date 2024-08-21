using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class HealthHandler : MonoBehaviour
{
    [SerializeField] PlayerManager_Depricated _playerManager;

    [Header("Health")]
    [SerializeField] int _remainingLives;
    [SerializeField] int _maxLives;
    [SerializeField] bool _isInvincible;
    [SerializeField] float _respawnDelay;

    [SerializeField] Color _fullColor;
    [SerializeField] Color _halfColor;
    [SerializeField] static event Action<GameObject> _onRespawn;

    public float RemainingLives { get { return _remainingLives; } }
    public float MaxLives { get { return _maxLives; } }
    public bool IsInvicible { get { return _isInvincible; } set { _isInvincible = value; } }
    public float RespawnDelay { get { return _respawnDelay; } set { _respawnDelay = value; } }

    public static Action<GameObject> OnRespawn { get { return _onRespawn; } set { _onRespawn = value; } }

    private void Awake()
    {
        SetComponents();
    }

    void OnEnable()
    {
        LocalMatchManager.onResetPlayers += ResetLives;
    }

    private void Start()
    {
        LocalMatchManager.onResetPlayers -= ResetLives;
    }
    public void SetComponents()
    {
        if(_playerManager != null)
        {
            _playerManager = GetComponent<PlayerManager_Depricated>();
        }
       
    }

    public void KillPlayer(float damage)
    {
        if(this.enabled == true)
        {
            _playerManager.PlayerStateMachine.IsDead = true;
            CheckLives();
        }
        else
        {
            return;
        }        
    }
    public void ResetLives()
    {
        _remainingLives = _maxLives;
    }
    public void CheckLives()
    {

        if (_remainingLives >= 1){

            _remainingLives--;
            StartCoroutine(RespawnPlayer());
        }
        else if(_remainingLives == 0)
        {
            _playerManager.Die();
        }
        else
        {
            return;
        }
    }
    public IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(_respawnDelay);
        _playerManager.PlayerStateMachine.IsDead = false;
       // _playerManager.PlayerStateMachine.CurrentStamina = _playerManager.PlayerStateMachine.MaxStamina;
        _onRespawn.Invoke(this.gameObject);
        StartCoroutine(StartRespawnInvincibility());
    }
    public IEnumerator StartRespawnInvincibility()
    {
        _isInvincible = true;
        GetComponent<SpriteRenderer>().color = _halfColor;
        yield return new WaitForSeconds(.25f);
        GetComponent<SpriteRenderer>().color = _fullColor;
        yield return new WaitForSeconds(.25f);
        GetComponent<SpriteRenderer>().color = _halfColor;
        yield return new WaitForSeconds(.25f);
        GetComponent<SpriteRenderer>().color = _fullColor;
        yield return new WaitForSeconds(.25f);
        GetComponent<SpriteRenderer>().color = _halfColor;
        yield return new WaitForSeconds(.25f);
        GetComponent<SpriteRenderer>().color = _fullColor;
        yield return new WaitForSeconds(.25f);
        GetComponent<SpriteRenderer>().color = _halfColor;
        yield return new WaitForSeconds(.25f);
        GetComponent<SpriteRenderer>().color = _fullColor;
        yield return new WaitForSeconds(.25f);
        GetComponent<SpriteRenderer>().color = _halfColor;
        yield return new WaitForSeconds(.25f);
        GetComponent<SpriteRenderer>().color = _fullColor;
        yield return new WaitForSeconds(.25f);
        GetComponent<SpriteRenderer>().color = _halfColor;
        yield return new WaitForSeconds(.25f);
        GetComponent<SpriteRenderer>().color = _fullColor;
        yield return new WaitForSeconds(.25f);
        GetComponent<SpriteRenderer>().color = _halfColor;
        yield return new WaitForSeconds(.25f);
        GetComponent<SpriteRenderer>().color = _fullColor;
        yield return new WaitForSeconds(.25f);
        GetComponent<SpriteRenderer>().color = _halfColor;
        yield return new WaitForSeconds(.25f);
        GetComponent<SpriteRenderer>().color = _fullColor;
        yield return new WaitForSeconds(.25f);
        GetComponent<SpriteRenderer>().color = _halfColor;
        yield return new WaitForSeconds(.25f);
        GetComponent<SpriteRenderer>().color = _fullColor;
        yield return new WaitForSeconds(.25f);
        GetComponent<SpriteRenderer>().color = _halfColor;
        yield return new WaitForSeconds(.25f);
        GetComponent<SpriteRenderer>().color = _fullColor;
        yield return new WaitForSeconds(.25f);
        GetComponent<SpriteRenderer>().color = _halfColor;
        yield return new WaitForSeconds(.25f);
        GetComponent<SpriteRenderer>().color = _fullColor;
        yield return new WaitForSeconds(.25f);
        GetComponent<SpriteRenderer>().color = _halfColor;
        yield return new WaitForSeconds(.25f);
        GetComponent<SpriteRenderer>().color = _fullColor;
        yield return new WaitForSeconds(.25f);
        _isInvincible = false;
    }
}
