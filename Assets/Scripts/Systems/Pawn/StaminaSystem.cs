using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaSystem : MonoBehaviour
{
    public PlayerStateMachine playerStateMachine;
    public int currentDodges;
    public int maxDodges;
    public int minDodges;
    public float replenishTime; // Time in seconds between each dodge refill
    public float replenishDelay;
    public bool isEnergized;
    public Action<int, bool> onDodgeAdded;
    public Action<int, bool> onDodgeRemoved;
    public Action<bool> onDodgeDepeleted;
    public Action<bool> onDodgeReplenished;
    public Action onDodgeReset;
    private Coroutine dodgeReplenishCoroutine;
    private Coroutine replenishDelayCoroutine;

    private void Awake()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    private void OnEnable()
    {
        playerStateMachine.OnDodge += SpendDodge;
        playerStateMachine.OnDeath += ResetDodge;
        playerStateMachine.OnEnergized += SetUnlimitedDodges;
    }

    private void OnDisable()
    {
        playerStateMachine.OnDodge -= SpendDodge;
        playerStateMachine.OnDeath -= ResetDodge;
        playerStateMachine.OnEnergized += SetUnlimitedDodges;
    }

    private void Start()
    {
        currentDodges = maxDodges;
    }

    public void SpendDodge(bool value)
    {
        if (value && !isEnergized)
        {
            if(currentDodges == maxDodges)
            {
                onDodgeDepeleted?.Invoke(true);
            }
            currentDodges--;
            onDodgeRemoved?.Invoke(currentDodges, false);
            if (replenishDelayCoroutine == null)
            {
                replenishDelayCoroutine = StartCoroutine(StartReplenishDelay());
            }
            else
            {
                if(replenishDelayCoroutine != null)
                {
                    StopCoroutine(replenishDelayCoroutine);
                }

                if(dodgeReplenishCoroutine != null)
                {
                    StopCoroutine(dodgeReplenishCoroutine);
                }

                replenishDelayCoroutine = StartCoroutine(StartReplenishDelay());
            }
            if (currentDodges <= 0)
            {
                currentDodges = 0; // Ensure it doesn't go below 0
                playerStateMachine.IsExhausted = true;
                playerStateMachine.OnExhausted?.Invoke(true);
            }
        }
    }

    private IEnumerator StartReplenishDelay()
    {
        yield return new WaitForSeconds(replenishDelay);
        dodgeReplenishCoroutine = StartCoroutine(ReplenishDodges());

    }

    private IEnumerator ReplenishDodges()
    {
        yield return new WaitForSeconds(replenishTime); // Wait for the replenish time
        playerStateMachine.IsExhausted = false;
        playerStateMachine.OnExhausted?.Invoke(false);
        currentDodges++; // Refill by 1
        onDodgeAdded?.Invoke(currentDodges, true);    
        currentDodges = Mathf.Clamp(currentDodges, minDodges, maxDodges); // Ensure it doesn't go over maxDodges
        if(currentDodges < maxDodges)
        {
            dodgeReplenishCoroutine = StartCoroutine(ReplenishDodges());
        }
        else
        {
            dodgeReplenishCoroutine = null; // Stop the coroutine when dodges are full
            onDodgeDepeleted?.Invoke(false);
            onDodgeReplenished?.Invoke(true);
        }
    }

    public void ResetDodge(bool value)
    {
        if (value)
        {
            onDodgeReset?.Invoke();
            currentDodges = maxDodges;
            playerStateMachine.IsExhausted = false;
            playerStateMachine.OnExhausted?.Invoke(false);
            if (replenishDelayCoroutine != null)
            {
                StopCoroutine(replenishDelayCoroutine);
            }

            if (dodgeReplenishCoroutine != null)
            {
                StopCoroutine(dodgeReplenishCoroutine);
            }
        }
    }

    public void SetUnlimitedDodges(bool value)
    {
        isEnergized = value;
    }

}
