using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeUIObserver : MonoBehaviour
{
    [SerializeField] PlayerManager _subject;
    [SerializeField] int _observerId;
    [SerializeField] Slider _dodgeSlider;

    private void Awake()
    {
        SetComponents();
    }
    private void OnEnable()
    {
        _subject.GetComponent<PlayerStateMachine>().OnDodge += UpdateDodgeBar;
        _subject.GetComponent<PlayerStateMachine>().OnDodgeRefill += UpdateDodgeBar;
    }

    public void UpdateDodgeBar()
    {
        _dodgeSlider.value = _subject.PlayerStateMachine.TotalDodges;
    }
    public void SetComponents()
    {
        if (GameObject.Find($"Player{_observerId}") != null)
        {
            _subject = GameObject.Find($"Player{_observerId}").GetComponent<PlayerManager>();
            _dodgeSlider = GetComponent<Slider>();
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
