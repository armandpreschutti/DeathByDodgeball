using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public bool hasOwner;
    public PlayerStateMachine owner;
    public int owningTeam;
    public bool isEquipped;
    public bool isBallActive;
    

    [Header("Components")]
    public Collider2D _col;
    public Rigidbody2D _rb;
    public SpriteRenderer _spriteRenderer;

    [Header("VFX")]
    public GameObject _explosionImpact;
    public Sprite _ballSprite;
    public Sprite _bombSprite;

    [Header("Variables")]
    public Transform _parent;
    public float _ballDamage;
    public float _knockBackPower;
    public Vector2 Trajectory;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerStateMachine>() != null)
        {
            PlayerStateMachine stateMachine = collision.GetComponent<PlayerStateMachine>();
            if (!hasOwner)
            {
                if (!stateMachine.IsEquipped)
                {
                    EquiptBall(stateMachine);
                }
            }
            else
            {
                if(stateMachine != owner && isBallActive && stateMachine.GetComponent<PawnManager>().teamId != owningTeam)
                {
                    if(stateMachine.IsCatching)
                    {
                        EquiptBall(stateMachine);
                    }
                    else if(!stateMachine.IsDead)
                    {
                        stateMachine.IsDead = true;
                        Destroy(gameObject);
                    }

                }
            }
        }
    }

    public void EquiptBall(PlayerStateMachine stateMachine)
    {
        hasOwner = true;
        owner = stateMachine;
        stateMachine.EquipBall(gameObject);
        isEquipped = true;
        owningTeam = stateMachine.GetComponent<PawnManager>().teamId;
        isBallActive = false;
    }

   /* public void SetBallActiveState(bool value)
    {
        isBallActive = value;
    }
*/
    public void SetTrajectory(bool value, Vector2 direction)
    {
        isBallActive = value;
        GetComponent<Rigidbody2D>().AddForce(direction, ForceMode2D.Impulse);
    }
}
