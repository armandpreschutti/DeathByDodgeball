using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnVFXHandler : MonoBehaviour
{
    private PlayerStateMachine playerStateMachine;
    public GameObject superVfx;
    public GameObject exhaustedVfx;
    public GameObject dodgeVfx;

    private void Awake()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    private void OnEnable()
    {
        playerStateMachine.OnDodge += SetDodgeVFX;
        playerStateMachine.OnSuperState += SetSuperVFX;
    }

    private void OnDisable()
    {
        playerStateMachine.OnDodge -= SetDodgeVFX;
        playerStateMachine.OnSuperState -= SetSuperVFX;
    }


    private void Update()
    {
        //SetSuperVFX(playerStateMachine.IsSuper);
        SetExhaustedVFX(playerStateMachine.IsExhausted);
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
}
