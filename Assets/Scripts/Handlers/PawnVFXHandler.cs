using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnVFXHandler : MonoBehaviour
{
    private PlayerStateMachine playerStateMachine;
    public ParticleSystem superVfx;
    public ParticleSystem exhaustedVfx;

    private void Start()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    private void Update()
    {
        SetSuperVFX(playerStateMachine.IsSuper);
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
}
