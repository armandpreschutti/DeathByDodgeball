using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocomotionHandler : MonoBehaviour
{
    public enum LocomotionState  {Walking, Dashing};

    public PlayerManager playerManager;
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float aimingSpeed = 2f;   
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDuration;
    [SerializeField] int totalDashes;
    [SerializeField] int maxDashs;
    [SerializeField] bool isDashing;
    [SerializeField] float dashRefillRate;

    Vector2 moveDirection = Vector2.zero;

    private void Awake()
    {   
        playerManager = GetComponent<PlayerManager>();  
       
    }
    private void Start()
    {
        totalDashes = maxDashs;
        if(transform.position.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
        }
    }
    void Update()
    {
        if (!isDashing && !playerManager.meleeHandler.isCatching)
        {
            moveDirection = playerManager.inputHandler.moveAction.ReadValue<Vector2>();
            playerManager.anim.SetFloat("MoveX", moveDirection.x);
            playerManager.anim.SetFloat("MoveY", moveDirection.y);
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            if (playerManager.meleeHandler.isAiming)
            {
                playerManager.rb.velocity = new Vector2(moveDirection.x * aimingSpeed, moveDirection.y * aimingSpeed);
            }
            else if (playerManager.meleeHandler.isCatching)
            {
                playerManager.rb.velocity = Vector2.zero;
            }
            else
            {
                playerManager.rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
            }
        }        
    }

    public void Dodge()
    {
        if(!isDashing 
            && !playerManager.meleeHandler.isAiming 
            && !playerManager.meleeHandler.isCatching
            && totalDashes != 0
            )
        {
            StartCoroutine(DashCoroutine());
            StartCoroutine(RefillDash());
        }
        else
        {
            return;
        }
    }

    IEnumerator DashCoroutine()
    {
        
        isDashing = true;
        playerManager.anim.SetBool("IsDodging", true);
        totalDashes -= 1;
        playerManager.dodgeSlider.value = totalDashes;
        float startTime = Time.time;

        while (Time.time < startTime + dashDuration)
        {
            playerManager.rb.velocity = moveDirection.normalized * dashSpeed;
            yield return null;
        }
        playerManager.anim.SetBool("IsDodging", false);
        isDashing = false;
    }
    IEnumerator RefillDash()
    {
        yield return new WaitForSeconds(dashRefillRate);
        totalDashes += 1;
        playerManager.dodgeSlider.value = totalDashes;
    }
    public void TestSub()
    {
        Debug.Log("Successfully subscribed to an event");
    }
}
