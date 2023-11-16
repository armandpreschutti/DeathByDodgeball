using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIObserver : MonoBehaviour
{
    [SerializeField] PlayerManager _playerManager;
    [SerializeField] int _subjectId;
    [SerializeField] Image _heart1;
    [SerializeField] Image _heart2;
    [SerializeField] Image _heart3;
    [SerializeField] TextMeshProUGUI _lastChanceText;
    [SerializeField] Image _backGround;

    private void Awake()
    {
        SetComponents();
    }

    private void OnEnable()
    {
        if (_playerManager != null)
        {
            _playerManager.GetComponent<PlayerStateMachine>().OnRespawn += UpdateLivesUI;
        }
        PlayerManager.OnPlayerDeath += CheckPlayerDeath;
        LocalMatchManager.onResetPlayers += ResetUIelements;
    }

    private void OnDisable()
    {
        if(_playerManager != null)
        {
            _playerManager.GetComponent<PlayerStateMachine>().OnRespawn -= UpdateLivesUI;
        }
        PlayerManager.OnPlayerDeath -= CheckPlayerDeath;
        LocalMatchManager.onResetPlayers -= ResetUIelements;
    }

    public void UpdateLivesUI()
    {
        switch(_playerManager.HealthHandler.RemainingLives)
        {
            case 0:
                StartCoroutine(RemoveLife(_heart1, true));
                break;
            case 1:
                StartCoroutine(RemoveLife(_heart2, false));
                break;
            case 2:
                StartCoroutine(RemoveLife(_heart3, false));
                break;
            default:
                break;
        }
    }

    public void CheckPlayerDeath(PlayerManager playerManager)
    { 
        if(playerManager == _playerManager)
        {
            _lastChanceText.text = "DEAD!";
        }
        else
        {
            return;
        }
    }
    public void SetComponents()
    {
        if (GameObject.Find($"Player{_subjectId}") != null)
        {
            _playerManager = GameObject.Find($"Player{_subjectId}").GetComponent<PlayerManager>();
            if(_playerManager.TeamId == 1)
            {
                _backGround.color = GameManager.GetInstance().team1Color;
            }
            else if( _playerManager.TeamId == 2)
            {
                _backGround.color = GameManager.GetInstance().team2Color;
            }
            else
            {
                return;
            }
            ResetUIelements();
            
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
    IEnumerator RemoveLife(Image heart, bool lastChanceActive)
    {
        heart.rectTransform.localScale += new Vector3 (.25f, .25f, .25f);
        heart.color = Color.red;
        yield return new WaitForSeconds(.5f);
        heart.gameObject.SetActive(false);
        if(lastChanceActive)
        {
            _lastChanceText.gameObject.SetActive(true);
        }
    }
    public void ResetUIelements()
    {
        _heart1.gameObject.SetActive(true);
        _heart2.gameObject.SetActive(true);
        _heart3.gameObject.SetActive(true);
        _lastChanceText.text = "Last Chance!";
        _lastChanceText.gameObject.SetActive(false);
    }

}
