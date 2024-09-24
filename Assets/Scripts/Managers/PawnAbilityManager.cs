using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnAbilityManager : MonoBehaviour
{
    PlayerStateMachine playerStateMachine;
    float originalMoveSpeed;
    float originalDodgeSpeed;
    float originalThrowRate;



    public Coroutine setFrozenState;
    public Action<bool> onFrozen;
    public Action<bool> onSuperFrozen;

    public Coroutine setEnergizedState;
    public Action<bool> onEnergized;
    public Action<bool> onSuperEnergized;

    public Coroutine setInvicibleState;
    public Action<bool> onInvicible;
    public Action<bool> onSuperInvicible;
    public float InvicibilityTime;

    private void Awake()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }
    private void OnEnable()
    {
        playerStateMachine.OnDeath += StopAllStates;
        playerStateMachine.OnRespawn += SetInvicibileState;
    }

    private void OnDisable()
    {
        playerStateMachine.OnDeath -= StopAllStates;
        playerStateMachine.OnRespawn -= SetInvicibileState;
    }
    private void Start()
    {
        originalMoveSpeed = playerStateMachine.MoveSpeed;
        originalDodgeSpeed = playerStateMachine.DodgeSpeed;
        originalThrowRate = playerStateMachine.ThrowPowerIncreaseRate;
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
        if (setEnergizedState != null)
        {
            StopCoroutine(setEnergizedState);
            playerStateMachine.IsExhausted = true;
            BroadCastEnergizedType(isSuperBall, false);
            BroadCastEnergizedType(!isSuperBall, false);
        }
        playerStateMachine.DestroyBall();
        playerStateMachine.MoveSpeed = isSuperBall ? 0 : speed;
        playerStateMachine.IsExhausted = true;
        playerStateMachine.CanCatch= isSuperBall ? false : true;
        yield return new WaitForSeconds(isSuperBall ? time * 1.5f : time);
        playerStateMachine.CanCatch = true;
        playerStateMachine.MoveSpeed = originalMoveSpeed;
        playerStateMachine.IsExhausted = false;
        BroadCastFrozenType(isSuperBall, false);
        BroadCastFrozenType(!isSuperBall, false);
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

    public void SetEnergizedState(bool isSuperBall, float energizedSpeed, float dodgeSpeed, float throwRate, float energizedTime)
    {
        if (setEnergizedState == null)
        {
            setEnergizedState = StartCoroutine(EnergizedStateCoroutine(isSuperBall, energizedSpeed, dodgeSpeed, throwRate, energizedTime));
        }
        else
        {
            if (setEnergizedState != null)
            {
                StopCoroutine(setEnergizedState);
            }

            setEnergizedState = StartCoroutine(EnergizedStateCoroutine(isSuperBall, energizedSpeed, dodgeSpeed, throwRate, energizedTime));
        }
    }

    public IEnumerator EnergizedStateCoroutine(bool isSuperBall, float speed, float dodgeSpeed, float throwRate, float time)
    {
        BroadCastEnergizedType(isSuperBall, true);
        if (setFrozenState != null)
        {
            StopCoroutine(setFrozenState);
            playerStateMachine.IsExhausted = false;
            playerStateMachine.CanCatch = true;
            BroadCastFrozenType(isSuperBall, false);
            BroadCastFrozenType(!isSuperBall, false);
        }
        playerStateMachine.MoveSpeed = speed;
        playerStateMachine.DodgeSpeed = dodgeSpeed;
        playerStateMachine.ThrowPowerIncreaseRate = isSuperBall? throwRate : originalThrowRate;
        if(isSuperBall)
        {
            playerStateMachine.OnEnergized?.Invoke(true);
        }
        yield return new WaitForSeconds(time);
        playerStateMachine.ThrowPowerIncreaseRate = originalThrowRate;
        playerStateMachine.DodgeSpeed = originalDodgeSpeed;
        playerStateMachine.MoveSpeed = originalMoveSpeed;
        if (isSuperBall)
        {
            playerStateMachine.OnEnergized?.Invoke(false);
        }
        BroadCastEnergizedType(isSuperBall, false);
        BroadCastEnergizedType(!isSuperBall, false);
    }

    public void StopAllStates(bool value)
    {
        if (setFrozenState != null)
        {
            StopCoroutine(setFrozenState);
            playerStateMachine.IsExhausted = false;
            playerStateMachine.MoveSpeed = originalMoveSpeed;
            playerStateMachine.CanCatch = true;
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
            playerStateMachine.ThrowPowerIncreaseRate = originalThrowRate;
        }
        if (setInvicibleState != null)
        {
            StopCoroutine(setInvicibleState);
            onInvicible?.Invoke(false);
            playerStateMachine.IsInvicible = false;
        }
    }

    public void SetInvicibileState()
    {
        if(setInvicibleState != null)
        {
            StopCoroutine(setInvicibleState);
        }
        setInvicibleState = StartCoroutine(InvicibleStateCoroutine());
    }


    public IEnumerator InvicibleStateCoroutine()
    {
        onInvicible?.Invoke(true);
        playerStateMachine.IsInvicible= true;
        yield return new WaitForSeconds(InvicibilityTime);
        playerStateMachine.IsInvicible = false;
        onInvicible?.Invoke(false);
    }

}
