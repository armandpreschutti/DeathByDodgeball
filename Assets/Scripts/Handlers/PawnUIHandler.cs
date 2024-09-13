using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnUIHandler : MonoBehaviour
{
    private StaminaSystem staminaSystem;

    public GameObject dodgeContainer;
    public GameObject dodge1;
    public GameObject dodge2;
    public GameObject dodge3;
    public float delayAfterReplenish = 1f;
    public Coroutine disableDodgeUI;

    private void Awake()
    {
        if(GetComponent<StaminaSystem>() != null)
        {
            staminaSystem = GetComponent<StaminaSystem>();
        }
        else
        {
            Debug.LogError("PawnUIHandler requires a Stamina System component");
            Destroy(this);
        }

    }

    private void OnEnable()
    {
        staminaSystem.onDodgeAdded += AddDodgeValue;
        staminaSystem.onDodgeRemoved+= RemoveDodgeValue;
        staminaSystem.onDodgeDepeleted += ActivateDodgeContainer;
        staminaSystem.onDodgeReset += ResetValues;
    }
    private void OnDisable()
    {
        staminaSystem.onDodgeAdded -= AddDodgeValue;
        staminaSystem.onDodgeRemoved -= RemoveDodgeValue;
        staminaSystem.onDodgeDepeleted -= ActivateDodgeContainer;
        staminaSystem.onDodgeReset -= ResetValues;
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

    public void ResetValues()
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
}
