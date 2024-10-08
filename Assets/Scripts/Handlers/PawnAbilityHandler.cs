using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnAbilityHandler : MonoBehaviour
{
    PlayerStateMachine playerStateMachine;
    float originalMoveSpeed;
    float originalDodgeSpeed;

    public Coroutine setFrozenState;
    public Action<bool> onFrozen;
    public Action<bool> onSuperFrozen;

    public Coroutine setEnergizedState;
    public Action<bool> onEnergized;
    public Action<bool> onSuperEnergized;

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
        originalMoveSpeed = playerStateMachine.MoveSpeed;
        originalDodgeSpeed = playerStateMachine.DodgeSpeed;
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
        playerStateMachine.MoveSpeed = originalMoveSpeed;
        playerStateMachine.IsExhausted = false;
        BroadCastFrozenType(isSuperBall, false);
    }

    private void BroadCastEnergizedType(bool isSuperBall, bool value)
    {
        if (isSuperBall)
        {
            onSuperEnergized?.Invoke(value);
        }
        else
        {
            onEnergized?.Invoke(value);
        }
    }

    public void SetEnergizedState(bool isSuperBall, float energizedSpeed, float dodgeSpeed, float energizedTime)
    {
        if (setEnergizedState == null)
        {
            setEnergizedState = StartCoroutine(EnergizedStateCoroutine(isSuperBall, energizedSpeed, dodgeSpeed, energizedTime));
        }
        else
        {
            if (setEnergizedState != null)
            {
                StopCoroutine(setEnergizedState);
            }

            setEnergizedState = StartCoroutine(EnergizedStateCoroutine(isSuperBall, energizedSpeed, dodgeSpeed, energizedTime));
        }
    }

    public IEnumerator EnergizedStateCoroutine(bool isSuperBall, float speed, float dodgeSpeed, float time)
    {
        BroadCastEnergizedType(isSuperBall, true);
        playerStateMachine.MoveSpeed = speed;
        playerStateMachine.DodgeSpeed = dodgeSpeed;
        if(isSuperBall)
        {
            playerStateMachine.OnEnergized?.Invoke(true);
        }
        yield return new WaitForSeconds(isSuperBall ? time * 1.5f : time);
        playerStateMachine.DodgeSpeed = originalDodgeSpeed;
        playerStateMachine.MoveSpeed = originalMoveSpeed;
        if (isSuperBall)
        {
            playerStateMachine.OnEnergized?.Invoke(false);
        }
        BroadCastEnergizedType(isSuperBall, false);
    }

    public void StopAllStates(bool value)
    {
        if (setFrozenState != null)
        {
            StopCoroutine(setFrozenState);
            playerStateMachine.IsExhausted = false;
            playerStateMachine.MoveSpeed = originalMoveSpeed;
            onSuperFrozen?.Invoke(false);
            onFrozen?.Invoke(false);
        }
        if(setEnergizedState!= null)
        {
            StopCoroutine(setEnergizedState);
            playerStateMachine.MoveSpeed = originalMoveSpeed;
            onEnergized?.Invoke(false);
            onSuperEnergized?.Invoke(false);    
            playerStateMachine.DodgeSpeed = originalDodgeSpeed;
        }
    }


}
