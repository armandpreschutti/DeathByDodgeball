using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MeleeHandler : MonoBehaviour
{
    public PlayerManager playerManager;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] InputAction fireAction;
    [SerializeField] InputAction catchAction;
    [SerializeField] Vector3 aimPosition;
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
        playerInput = GetComponent<PlayerInput>();
        fireAction = playerInput.actions["Fire"];
        catchAction = playerInput.actions["Catch"];
    }

    private void OnEnable()
    {
        fireAction.started += StartAiming;
        fireAction.canceled += StopAiming;
        catchAction.performed += Catch;
    }

    private void OnDisable()
    {
        fireAction.started -= StartAiming;
        fireAction.canceled -= StopAiming;
        catchAction.performed -= Catch;
    }
    private void Start()
    {
        currentThrowPower = minThrowPower;
    }

    public void StartAiming(InputAction.CallbackContext context)
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

    public void StopAiming(InputAction.CallbackContext context)
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
            //playerManager.equippedBall.transform.localPosition = - aimingDirection.normalized;

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
            aimingDirection = (playerManager.target.position - transform.position).normalized;
            playerManager.equippedBall.GetComponent<Rigidbody2D>().AddForce(aimingDirection * currentThrowPower, ForceMode2D.Impulse);
            playerManager.equippedBall.GetComponent<ProjectileHandler>().ActivateBall(currentThrowPower);
            playerManager.ActivateBall();
        }
        else
        {
            return;
        }
    }

    public void Catch(InputAction.CallbackContext context)
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
   



}
