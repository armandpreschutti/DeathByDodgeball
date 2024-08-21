using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public bool hasOwner;
    public PlayerStateMachine owner;
    public bool owningTeam;

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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerStateMachine>() != null)
        {
            PlayerStateMachine stateMachine = collision.GetComponent<PlayerStateMachine>();
            if (!stateMachine.IsEquipped)
            {
                EquiptBall(stateMachine);
            }
        }
    }

    private void Update()
    {
        SetTrajecctory();
    }

    public void EquiptBall(PlayerStateMachine stateMachine)
    {
        hasOwner = true;
        owner = stateMachine;
        stateMachine.EquipBall(gameObject);
    }

    public void ActivateBall()
    {
        owner.UnequipBall(gameObject);
    }

    public void SetTrajecctory()
    {

    }
}
