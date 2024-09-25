using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PawnUIHandler : MonoBehaviour
{
    private StaminaSystem staminaSystem;
    private PlayerStateMachine playerStateMachine;
    private PawnManager pawnManager;
    public Animator anim;
    public GameObject dodgeContainer;
    public GameObject dodge1;
    public GameObject dodge2;
    public GameObject dodge3;
    public GameObject throwPowerBar;
    public Slider throwPowerSlider;
    public float delayAfterReplenish = 1f;
    public Coroutine disableDodgeUI;
    public TextMeshProUGUI playerTag;
    public float playerTagDisplayTime;

    private void Awake()
    {
        if(GetComponent<StaminaSystem>() != null)
        {
            staminaSystem = GetComponent<StaminaSystem>();
        }
        else
        {
            Debug.LogError("PawnUIHandler requires a Stamina System component");
        }
        playerStateMachine = GetComponent<PlayerStateMachine>();
        pawnManager = GetComponent<PawnManager>();
        playerTag.text = pawnManager.playerName;
        playerTag.color = pawnManager.pawnColor;
    }

    private void OnEnable()
    {

        if (staminaSystem!= null)
        {
            staminaSystem.onDodgeAdded += AddDodgeValue;
            staminaSystem.onDodgeRemoved += RemoveDodgeValue;
            staminaSystem.onDodgeDepeleted += ActivateDodgeContainer;
            staminaSystem.onDodgeReset += ResetDodgeValues;
        }
        playerStateMachine.OnAim += SetThrowBarState;
        playerStateMachine.OnSuperState += SetSuperThrowBarState;
        playerStateMachine.OnRespawn += TriggerPlayerTagDisplay;
        MatchInstanceManager.onEnablePawnControl += TriggerPlayerTagDisplay;
    }

    private void OnDisable()
    {   
        if(staminaSystem!= null)
        {
            staminaSystem.onDodgeAdded -= AddDodgeValue;
            staminaSystem.onDodgeRemoved -= RemoveDodgeValue;
            staminaSystem.onDodgeDepeleted -= ActivateDodgeContainer;
            staminaSystem.onDodgeReset -= ResetDodgeValues;
        }
        playerStateMachine.OnAim -= SetThrowBarState;
        playerStateMachine.OnSuperState -= SetSuperThrowBarState;
        playerStateMachine.OnRespawn -= TriggerPlayerTagDisplay;
        MatchInstanceManager.onEnablePawnControl -= TriggerPlayerTagDisplay;
    }

    private void Update()
    {
        if (playerStateMachine.IsAiming)
        {
            throwPowerSlider.value = playerStateMachine.CurrentThrowPower;
        }
    }

    public void RemoveDodgeValue(int dodges, bool value)
    {
        switch(dodges)
        {
            case 0:
                dodge1.SetActive(false);
                break;
            case 1:
                dodge2.SetActive(false);
                break;
            case 2:
                dodge3.SetActive(false);
                break;
        }
    }

    public void AddDodgeValue(int dodges, bool value)
    {
        switch (dodges)
        {
            case 1:
                dodge1.SetActive(true);
                break;
            case 2:
                dodge2.SetActive(true);
                break;
            case 3:
                dodge3.SetActive(true);
                break;
        }
    }

    public void ActivateDodgeContainer(bool value)
    {
        if(value)
        {
            dodgeContainer.SetActive(true);
            if (disableDodgeUI != null)
            {
                StopCoroutine(disableDodgeUI);
            }
        }
        else
        {
            disableDodgeUI = StartCoroutine(DisableDodgeUI());
        }
        
    }

    public IEnumerator DisableDodgeUI()
    {
        yield return new WaitForSeconds(delayAfterReplenish);
        dodgeContainer.SetActive(false);
    }

    public void ResetDodgeValues()
    {
        dodgeContainer.SetActive(false);
        dodge1.SetActive(true);
        dodge2.SetActive(true);
        dodge3.SetActive(true);
        if (disableDodgeUI != null)
        {
            StopCoroutine(disableDodgeUI);
        }
    }

    public void SetThrowBarState(bool value)
    {
        throwPowerBar.SetActive(value);
        throwPowerSlider.minValue = playerStateMachine.MinThrowPower;
        throwPowerSlider.maxValue = playerStateMachine.MaxThrowPower;
        throwPowerSlider.value = playerStateMachine.CurrentThrowPower;
        RectTransform rectTransform = throwPowerBar.GetComponent<RectTransform>();
        Vector2 newAnchoredPosition = rectTransform.anchoredPosition;
        newAnchoredPosition.x = transform.position.x > 0 ? 400 : -400;
        rectTransform.anchoredPosition = newAnchoredPosition;
    }
    public void SetSuperThrowBarState(bool value)
    {
        if (value)
        {
            anim.Play("SuperThrowBarFlash");
        }
        else
        {
            anim.Play("Idle");
        }
    }

    public void TriggerPlayerTagDisplay()
    {
        StartCoroutine(DisplayPlayerTagCoroutine());
    }

    public IEnumerator DisplayPlayerTagCoroutine()
    {
        playerTag.gameObject.SetActive(true);
        yield return new WaitForSeconds(playerTagDisplayTime);
        playerTag.gameObject.SetActive(false);
    }

}
