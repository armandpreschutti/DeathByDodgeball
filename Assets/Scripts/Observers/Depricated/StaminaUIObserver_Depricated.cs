using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaUIObserver : MonoBehaviour
{

    [SerializeField] PlayerManager_Depricated _playerManager;
    [SerializeField] int _subjectId;
    [SerializeField] Slider _staminaSlider;

    private void Awake()
    {
        _staminaSlider.value = _staminaSlider.maxValue;
    }
    private void OnEnable()
    {
/*        _playerManager.GetComponent<PlayerStateMachine>().OnDodge += UpdateStaminaBar;
        _playerManager.GetComponent<PlayerStateMachine>().OnThrow += UpdateStaminaBar;
        _playerManager.GetComponent<PlayerStateMachine>().OnStaminaReplenish += UpdateStaminaBar;
        _playerManager.GetComponent<PlayerStateMachine>().OnStaminaFilled += TurnOffStaminaBar;*/
    }
    private void OnDisable()
    {
/*        _playerManager.GetComponent<PlayerStateMachine>().OnDodge -= UpdateStaminaBar;
        _playerManager.GetComponent<PlayerStateMachine>().OnThrow -= UpdateStaminaBar;
        _playerManager.GetComponent<PlayerStateMachine>().OnStaminaReplenish -= UpdateStaminaBar;
        _playerManager.GetComponent<PlayerStateMachine>().OnStaminaFilled -= TurnOffStaminaBar;*/
    }

    public void UpdateStaminaBar()
    {
       /* if(!_playerManager.GetComponent<PlayerStateMachine>().IsInvicible)
        {
            bool flipped = _playerManager.transform.position.x > 0f;
            _staminaSlider.direction = flipped ? Slider.Direction.RightToLeft : Slider.Direction.LeftToRight;
            _staminaSlider.gameObject.SetActive(true);
          //  _staminaSlider.value = _playerManager.PlayerStateMachine.CurrentStamina;
        }        */
    }

    public void TurnOffStaminaBar()
    {
        _staminaSlider.gameObject.SetActive(false);
    }
  
}
