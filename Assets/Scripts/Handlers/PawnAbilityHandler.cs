using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnAbilityHandler : MonoBehaviour
{
    PlayerStateMachine playerStateMachine;
    float originalSpeed;
    public Coroutine setFrozenState;
    public Action<bool> onFrozen;
    public Action<bool> onSuperFrozen;

    private void Awake()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }
    private void OnEnable()
    {
        playerStateMachine.OnDeath += StopAllStates;
    }

    private void OnDisable()
    {
        playerStateMachine.OnDeath -= StopAllStates;
    }
    private void Start()
    {
        originalSpeed = playerStateMachine.MoveSpeed;
    }
    public void SetFrozenState(bool isSuperBall, float frozenSpeed, float frozenTime)
    {
        if (setFrozenState == null)
        {
            setFrozenState = StartCoroutine(FrozenStateCoroutine(isSuperBall, frozenSpeed, frozenTime));
        }
        else
        {
            if (setFrozenState != null)
            {
                StopCoroutine(setFrozenState);
            }

            setFrozenState = StartCoroutine(FrozenStateCoroutine(isSuperBall, frozenSpeed, frozenTime));
        }
    }

    public IEnumerator FrozenStateCoroutine(bool isSuperBall, float speed, float time)
    {
        BroadCastFrozenType(isSuperBall, true);
        playerStateMachine.MoveSpeed = isSuperBall ? 0 : speed;
        playerStateMachine.IsExhausted = true;
        yield return new WaitForSeconds(isSuperBall ? time * 1.5f : time);
        playerStateMachine.MoveSpeed = originalSpeed;
        playerStateMachine.IsExhausted = false;
        BroadCastFrozenType(isSuperBall, false);
    }

    private void BroadCastFrozenType(bool isSuperBall, bool value)
    {
        if (isSuperBall)
        {
            onSuperFrozen?.Invoke(value);
        }
        else
        {
            onFrozen?.Invoke(value);
        }
    }

    public void StopAllStates(bool value)
    {
        if (setFrozenState != null)
        {
            StopCoroutine(setFrozenState);
            playerStateMachine.IsExhausted = false;
            playerStateMachine.MoveSpeed = originalSpeed;
            onSuperFrozen?.Invoke(false);
            onFrozen?.Invoke(false);
        }
    }
}
