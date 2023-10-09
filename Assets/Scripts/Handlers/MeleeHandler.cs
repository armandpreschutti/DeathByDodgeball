using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MeleeHandler : MonoBehaviour
{/*
    public PlayerManager playerManager;
    [SerializeField] float minThrowPower = 1f;
    [SerializeField] float maxThrowPower = 40f;
    [SerializeField] float currentThrowPower = 0f;
    [SerializeField] float throwPowerIncreaseRate;
    public bool isAiming;
    public bool isCatching;
    [SerializeField] float catchDuration;
    [SerializeField] Vector3 aimingDirection;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    private void Start()
    {
        currentThrowPower = minThrowPower;
    }
    public void Update()
    {
        aimingDirection = (playerManager.target.position - transform.position).normalized;
    }
    public void StartAiming()
    {
        if(playerManager.currentInventoryState == PlayerManager.InventoryState.equipped)
        {
            isAiming = true;
            playerManager.anim.SetBool("IsAiming", true);
            playerManager.throwPowerBar.SetActive(true);
            StartCoroutine(PrepareThrow());
        }
        else
        {
            return;
        }
    }

    public void StopAiming()
    {
        isAiming = false;
        playerManager.anim.SetBool("IsAiming", false);
        playerManager.throwPowerBar.SetActive(false);
        ThrowBall();
        currentThrowPower = minThrowPower;
    }

    IEnumerator PrepareThrow()
    {
        while (isAiming)
        {
            
            if (currentThrowPower >= maxThrowPower)
            {
               
                currentThrowPower = maxThrowPower;
                playerManager.throwPowerBar.GetComponent<Slider>().fillRect.GetComponent<Image>().color = Color.red;
            }
            else
            {
                currentThrowPower += throwPowerIncreaseRate * Time.deltaTime;
                playerManager.throwPowerBar.GetComponent<Slider>().value = currentThrowPower;
            }

            yield return null;
        }
        playerManager.throwPowerBar.GetComponent<Slider>().value = minThrowPower;
        playerManager.throwPowerBar.GetComponent<Slider>().fillRect.GetComponent<Image>().color = Color.white;
    }

    public void ThrowBall()
    {
        if (playerManager.currentInventoryState == PlayerManager.InventoryState.equipped)
        {
            
            
            //playerManager.equippedBall.GetComponent<ProjectileHandler>().ActivateBall(currentThrowPower, aimingDirection);
            //playerManager.ActivateBall();
        }
        else
        {
            return;
        }
    }

    public void Catch()
    {
        StartCoroutine(CatchBall());
    }

    IEnumerator CatchBall()
    {
        if(playerManager.currentInventoryState == PlayerManager.InventoryState.unequipped)
        {
            isCatching = true;
            GetComponent<Animator>().SetBool("IsCatching", true);
            yield return new WaitForSeconds(catchDuration);
            GetComponent<Animator>().SetBool("IsCatching", false);
            isCatching = false;
        }
        else
        {

            yield return null;
        }
        
    }
   


*/
}
