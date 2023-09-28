using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    public PlayerManager playerManager;
    [SerializeField] float health;
    [SerializeField] bool isDead;
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float minhealth = 0f;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    void OnEnable()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        
        if(health - damage <= minhealth)
        {
            health = minhealth;
            UpdateHealthBar(); 
            playerManager.Die();
            return;
        }
        else
        {
            health -= damage;
        }
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        playerManager.healthSlider.value = health;
    }
}
