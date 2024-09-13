using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnSFXHandler : MonoBehaviour
{
    private PlayerStateMachine _playerStateMachine;
    private AudioSource _audioSource;
    public AudioClip DodgeSfx;
    public AudioClip ThrowSfx;
    public AudioClip CatchSfx;
    public AudioClip DeathSFX;
    public AudioClip SuperStateSfx;

    private void Awake()
    {
        _playerStateMachine = GetComponent<PlayerStateMachine>();
        _audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        _playerStateMachine.OnDodge += PlayDodgeSFX;
        _playerStateMachine.OnThrow += PlayThrowSFX;
        _playerStateMachine.OnBallCaught += PlayCatchSFX;
        _playerStateMachine.OnDeath += PlayDeathSFX;
        _playerStateMachine.OnSuperState += PlaySuperSFX;
    }
    private void OnDisable()
    {
        _playerStateMachine.OnDodge -= PlayDodgeSFX;
        _playerStateMachine.OnThrow -= PlayThrowSFX;
        _playerStateMachine.OnBallCaught -= PlayCatchSFX;
        _playerStateMachine.OnDeath -= PlayDeathSFX;
        _playerStateMachine.OnSuperState -= PlaySuperSFX;
    }




    public void PlayDodgeSFX(bool value)
    {
        if (value)
        {
            _audioSource.PlayOneShot(DodgeSfx);
        }

    }   
    public void PlayThrowSFX(bool value)
    {
        if (value)
        {
            _audioSource.PlayOneShot(ThrowSfx);
        }

    }   
    public void PlayCatchSFX()
    {
        _audioSource.PlayOneShot(CatchSfx);
    }   
    public void PlayDeathSFX(bool value)
    {
        if (value)
        {
            _audioSource.PlayOneShot(DeathSFX);
        }

    }
    public void PlaySuperSFX(bool value)
    {

    }   

}
