using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarrierHandler : MonoBehaviour
{
    public int owningTeam;
    public GameObject selfDestructVFX;
    public int health = 1;
    public Slider healthBar;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            BallManager ball = collision.gameObject.GetComponent<BallManager>(); 
            if(ball.owningTeam != owningTeam)
            {                
                Instantiate(selfDestructVFX, collision.transform.position, Quaternion.identity, null);

                health--;
                healthBar.value = health;
                if(health <= 0)
                {
                    Destroy(gameObject);
                }
                Destroy(collision.gameObject);
            }

        }
    }
}
