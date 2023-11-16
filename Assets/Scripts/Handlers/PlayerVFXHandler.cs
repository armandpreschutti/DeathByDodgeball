using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFXHandler : MonoBehaviour
{

    [SerializeField] PlayerManager _playerManager;
    [SerializeField] ParticleSystem _dodgeVFX;
    [SerializeField] ParticleSystem _hurtVFX;

    private void OnEnable()
    {
        if (_playerManager != null)
        {
            _playerManager = GetComponent<PlayerManager>();
        }
    }
    private void OnDisable()
    {

    }

    public void PlayDodgeVFX()
    {
        _dodgeVFX.Play();
    }

    public void PlayHurtVFX()
    {
        _hurtVFX.Play();
    }
}
