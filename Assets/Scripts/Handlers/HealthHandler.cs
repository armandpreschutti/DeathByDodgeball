using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class HealthHandler : MonoBehaviour
{
    [SerializeField] PlayerManager _playerManager;

    [Header("Health")]
    [SerializeField] float _currentHealth;
    [SerializeField] float _maxHealth = 100f;
    [SerializeField] float _minhealth = 0f;

    public float CurrentHealth { get { return _currentHealth; } }
    public float hitDuration;

    private void Awake()
    {
        _playerManager = GetComponent<PlayerManager>();
    }

    void OnEnable()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _playerManager.PlayerStateMachine.IsHurt = true;
        _currentHealth -= damage;

        if (_currentHealth <= _minhealth)
        {
            _currentHealth = _minhealth;
            //Die();
        }
    }
}
