using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HealthHandler : MonoBehaviour
{
    public PlayerManager playerManager;
    [SerializeField] float health;
    [SerializeField] bool isDead;

    [SerializeField] float maxHealth = 100f;
    [SerializeField] float minhealth = 0f;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        
        if(health - damage <= minhealth)
        {
            health = minhealth;
            Die();
        }
        else
        {
            health -= damage;
        }
        UpdateHealthBar();
       /* if (health <= minhealth)
        {
            health = minhealth;
            Die();
        }*/
    }

    void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        transform.Rotate(Vector3.forward, 90);
        if (gameObject.name == "Player1")
        {
            GameObject.Find("GameplaySettings").GetComponent<GameplaySettings>().GameOver("Player 2 Wins!");
        }
        else if (gameObject.name == "Player2")
        {
            GameObject.Find("GameplaySettings").GetComponent<GameplaySettings>().GameOver("Player 1 Wins!");
        }
    }

    void UpdateHealthBar()
    {
        playerManager.healthSlider.value = health;
    }
}
