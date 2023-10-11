using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIObserver : MonoBehaviour
{
    [SerializeField] PlayerManager _subject;
    [SerializeField] int _subjectId;
    [SerializeField] Slider _heathSlider;

    private void Awake()
    {
        SetComponents();
    }
    private void OnEnable()
    {
        _subject.GetComponent<PlayerStateMachine>().OnHurt += UpdateHealthBar;
    }

    public void UpdateHealthBar()
    {
        _heathSlider.value = _subject.HealthHandler.CurrentHealth;
    }
    public void SetComponents()
    {
        if (GameObject.Find($"Player{_subjectId}") != null)
        {
            _subject = GameObject.Find($"Player{_subjectId}").GetComponent<PlayerManager>();
            _heathSlider= GetComponent<Slider>();
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
