using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFXHandler : MonoBehaviour
{
    [SerializeField] PlayerManager _playerManager;
    [SerializeField] AudioClip _currentSFX;
    [SerializeField] AudioClip _hurtSFX;
    [SerializeField] AudioClip _throwSFX;
    [SerializeField] AudioClip _equipSFX;
    [SerializeField] AudioClip _catchSFX;
    [SerializeField] AudioClip _dodgeSFX;
    [SerializeField] AudioClip _deathSFX;

    private void OnEnable()
    {
        SetComponents();
        _playerManager.GetComponent<PlayerStateMachine>().OnThrow += PlayThrowSFX;
        _playerManager.GetComponent<PlayerStateMachine>().OnEquip += PlayEquipSFX;
        _playerManager.GetComponent<PlayerStateMachine>().OnDeath += PlayDeathSFX;
        _playerManager.GetComponent<PlayerStateMachine>().OnHurt += PlayHurtSFX;
        _playerManager.GetComponent<PlayerStateMachine>().OnDodge += PlayDodgeSFX;
        _playerManager.GetComponent<PlayerStateMachine>().OnCatch += PlayCatchSFX;
    }
    private void OnDisable()
    {
        _playerManager.GetComponent<PlayerStateMachine>().OnThrow -= PlayThrowSFX;
        _playerManager.GetComponent<PlayerStateMachine>().OnEquip -= PlayEquipSFX;
        _playerManager.GetComponent<PlayerStateMachine>().OnDeath -= PlayDeathSFX;
        _playerManager.GetComponent<PlayerStateMachine>().OnHurt -= PlayHurtSFX;
        _playerManager.GetComponent<PlayerStateMachine>().OnDodge -= PlayDodgeSFX;
        _playerManager.GetComponent<PlayerStateMachine>().OnCatch -= PlayCatchSFX;
    }

    public void PlayHurtSFX()
    {
        _playerManager.PlayerAudio.clip = _hurtSFX;
        _playerManager.PlayerAudio.Play();
    }

    public void PlayThrowSFX()
    {
        _playerManager.PlayerAudio.clip = _throwSFX;
        _playerManager.PlayerAudio.Play();
    }

    public void PlayCatchSFX()
    {
        _playerManager.PlayerAudio.clip = _catchSFX;
        _playerManager.PlayerAudio.Play();
    }

    public void PlayEquipSFX()
    {
        _playerManager.PlayerAudio.clip = _equipSFX;
        _playerManager.PlayerAudio.Play();
    }

    public void PlayDodgeSFX()
    {
        _playerManager.PlayerAudio.clip = _dodgeSFX;
        _playerManager.PlayerAudio.Play();
    }

    public void PlayDeathSFX()
    {
        Debug.LogWarning("Death sound function was called");
        _playerManager.PlayerAudio.clip = _deathSFX;
        _playerManager.PlayerAudio.Play();
    }

    public void SetComponents()
    {
        if(_playerManager!= null)
        {
            _playerManager = GetComponent<PlayerManager>();
        }
        else
        {
            Debug.Log("AudioSource cannot find PlayerManager");
            return;
        }
    }
}
