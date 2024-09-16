using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnVFXHandler : MonoBehaviour
{
    private PlayerStateMachine playerStateMachine;
    public GameObject superVfx;
    public GameObject exhaustedVfx;
    public GameObject dodgeVfx;
    public GameObject healVfx;

    private void Awake()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    private void OnEnable()
    {
        playerStateMachine.OnDodge += SetDodgeVFX;
        playerStateMachine.OnExhausted += SetExhaustedVFX;
        playerStateMachine.OnSuperState += SetSuperVFX;
        playerStateMachine.OnHeal += SetHealVFX;
    }

    private void OnDisable()
    {
        playerStateMachine.OnDodge -= SetDodgeVFX;
        playerStateMachine.OnExhausted -= SetExhaustedVFX;
        playerStateMachine.OnSuperState -= SetSuperVFX;
        playerStateMachine.OnHeal -= SetHealVFX;
    }

    public void SetSuperVFX(bool valule)
    {
        superVfx.gameObject.SetActive(valule);

    }

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
}
