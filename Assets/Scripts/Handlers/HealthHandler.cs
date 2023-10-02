using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class HealthHandler : MonoBehaviour
{
    public PlayerManager playerManager;
    [SerializeField] float health;
    [SerializeField] bool isDead;
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float minhealth = 0f;
    public bool isHit;

    public float hitDuration;

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
            StartCoroutine(HitCoroutine());
            UpdateHealthBar();
            health -= damage;
        }
       
    }

    IEnumerator HitCoroutine()
    {

        isHit = true;
        playerManager.anim.SetBool("Hit", true);
        playerManager.anim.Play("Hit");
        yield return new WaitForSeconds(hitDuration);  
        isHit = false;
        playerManager.anim.SetBool("Hit", false);
    }
    void UpdateHealthBar()
    {
        playerManager.healthSlider.value = health;
    }
}
