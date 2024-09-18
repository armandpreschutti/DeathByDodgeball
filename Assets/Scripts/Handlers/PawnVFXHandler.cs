using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnVFXHandler : MonoBehaviour
{
    private PlayerStateMachine playerStateMachine;
    private PawnAbilityHandler pawnAbilityHandler;
    public Animator anim;
    public GameObject superVfx;
    public GameObject exhaustedVfx;
    public GameObject dodgeVfx;
    public GameObject healVfx;
    public GameObject frozenVfx;
    public GameObject superFrozenVfx;
    public GameObject energizedVfx;
    public GameObject superEnergizedVfx;


    private void Awake()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
        pawnAbilityHandler = GetComponent<PawnAbilityHandler>();
    }

    private void OnEnable()
    {
        playerStateMachine.OnDodge += SetDodgeVFX;
        playerStateMachine.OnExhausted += SetExhaustedVFX;
        //playerStateMachine.OnSuperState += SetSuperVFX;
        playerStateMachine.OnHeal += SetHealVFX;
        pawnAbilityHandler.onFrozen += SetFrozenVFX;
        pawnAbilityHandler.onSuperFrozen += SetSuperFrozenVFX;
        pawnAbilityHandler.onEnergized += SetEnergizedVFX;
        pawnAbilityHandler.onSuperEnergized += SetSuperEnergizedVFX;
    }

    private void OnDisable()
    {
        playerStateMachine.OnDodge -= SetDodgeVFX;
        playerStateMachine.OnExhausted -= SetExhaustedVFX;
       // playerStateMachine.OnSuperState -= SetSuperVFX;
        playerStateMachine.OnHeal -= SetHealVFX;
        pawnAbilityHandler.onFrozen -= SetFrozenVFX;
        pawnAbilityHandler.onSuperFrozen -= SetSuperFrozenVFX;
        pawnAbilityHandler.onEnergized -= SetEnergizedVFX;
        pawnAbilityHandler.onSuperEnergized -= SetSuperEnergizedVFX;
    }

/*    public void SetSuperVFX(bool valule)
    {
        superVfx.gameObject.SetActive(valule);

    }*/

    public void SetExhaustedVFX(bool valule)
    {
        exhaustedVfx.gameObject.SetActive(valule);

    }

    public void SetDodgeVFX(bool value)
    {
        if (value)
        {
            Instantiate(dodgeVfx, transform.position, Quaternion.identity, null);
        }
    }

    public void SetHealVFX()
    {
        healVfx.gameObject.SetActive(true);
    }

    public void SetFrozenVFX(bool value)
    {
        frozenVfx.gameObject.SetActive(value);
    }

    public void SetSuperFrozenVFX(bool value)
    {
        SetFrozenVFX(value);
        superFrozenVfx.gameObject.SetActive(value);
        anim.speed = value ? 0 : 1;
    }

    public void SetEnergizedVFX(bool value)
    {
        anim.speed = value ? 2 : 1;
        energizedVfx.gameObject.SetActive(value);
    }

    public void SetSuperEnergizedVFX(bool value)
    {
        SetEnergizedVFX(value);
        superEnergizedVfx.gameObject.SetActive(value);
    }
}
