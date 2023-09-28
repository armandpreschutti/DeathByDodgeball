using System.Collections;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    public enum BallState { inactive, active }
    public BallState currentState;
/*    public Vector3 targetDirection;*/
    public float ballDamage;
    public float collsionReactivationTime;

    private void Start()
    {
        currentState = BallState.inactive;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (currentState)
        {
            case BallState.inactive:
                if(transform.parent != null)
                {
                    GetComponentInParent<BallSpawnHandler>().TriggerRespawn();
                }
                EquipBall(collision);
                break;
            case BallState.active:
                DamagePlayer(collision);
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }

    public void EquipBall(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.GetComponent<PlayerManager>().currentInventoryState == PlayerManager.InventoryState.unequipped)
            {
                ballDamage = 0;
                Physics2D.IgnoreCollision(collision, GetComponent<Collider2D>());
                GetComponent<ProjectileHandler>().currentState = BallState.inactive;
                transform.parent = collision.transform;
                GetComponent<Collider2D>().enabled = false;
                GetComponent<Rigidbody2D>().simulated = false;
                collision.GetComponent<PlayerManager>().EquipBall(gameObject);

            }
        }
        else
        {
            return;
        }        
    }

    public void DamagePlayer(Collider2D collision)
    {
        if (!collision.GetComponent<MeleeHandler>().isCatching)
        {
            collision.GetComponent<HealthHandler>().TakeDamage(ballDamage);
            Destroy(gameObject);
        }
        else
        {
            EquipBall(collision);
        }
    }

    public void ActivateBall(float power, Vector3 aimingDirection)
    {
        GetComponent<Rigidbody2D>().simulated = true;
        ballDamage = power;
        GetComponent<Rigidbody2D>().AddForce(aimingDirection * power, ForceMode2D.Impulse);
        Debug.Log(aimingDirection * power);
        StartCoroutine(ReactivateCollision(transform.parent.GetComponent<Collider2D>()));
        GetComponent<ProjectileHandler>().currentState = BallState.active;
        GetComponent<Collider2D>().enabled = true;
        transform.parent = null;
        
    }   
    IEnumerator ReactivateCollision(Collider2D collider)
    {
        yield return new WaitForSeconds(collsionReactivationTime);
        Physics2D.IgnoreCollision(collider, GetComponent<Collider2D>(), false);
    }
}
