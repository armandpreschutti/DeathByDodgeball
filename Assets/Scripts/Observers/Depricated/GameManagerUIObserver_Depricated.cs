using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerUIObserver : MonoBehaviour
{
    public static event Action onMatchCountdownEnd;
    public static event Action<Scene> onSceneTransitionStart;
    public static event Action<Scene> onSceneTransitionEnd;
    [SerializeField] AudioClip _getReadySFX;
    [SerializeField] AudioClip _countSFX;
    [SerializeField] AudioClip _startMatchSFX;

    private void OnEnable()
    {
        GameManager_Depricated.onStartSceneTransition+= StartSceneTransition;
        GameManager_Depricated.onEndSceneTransition += EndSceneTransition;
        LocalMatchManager.onMatchCountdown += StartMatchCountdown;
        LocalMatchManager.onMatchOver += PlayStartMatchSFX;
    }
    private void OnDisable()
    {
        GameManager_Depricated.onStartSceneTransition-= StartSceneTransition;
        GameManager_Depricated.onEndSceneTransition -= EndSceneTransition;
        LocalMatchManager.onMatchCountdown -= StartMatchCountdown;
        LocalMatchManager.onMatchOver -= PlayStartMatchSFX;
    }

    public void StartSceneTransition()
    {
        GameManager_Depricated.GetInstance().Anim.Play("Start");
    }
    public void EndSceneTransition()
    {
        GameManager_Depricated.GetInstance().Anim.Play("End");
    }

    public void StartMatchCountdown()
    {
        GameManager_Depricated.GetInstance().Anim.Play("MatchCountdown");
    }

    public void BroadcastMatchCountdownEnd()
    {
        onMatchCountdownEnd?.Invoke();
    }

    public void BroadcastSceneTransitionStart()
    {
        onSceneTransitionStart?.Invoke(SceneManager.GetActiveScene());
    }
    public void BroadcastSceneTransitionEnd()
    {
        onSceneTransitionEnd?.Invoke(SceneManager.GetActiveScene());
    }


    public void PlayGetReadySFX()
    {
        GameManager_Depricated.GetInstance().GameManagerAudio.clip = _getReadySFX;
        GameManager_Depricated.GetInstance().GameManagerAudio.Play();
    }
    public void PlayCountSFX()
    {
        GameManager_Depricated.GetInstance().GameManagerAudio.clip = _countSFX;
        GameManager_Depricated.GetInstance().GameManagerAudio.Play();
    }
    public void PlayStartMatchSFX()
    {
        GameManager_Depricated.GetInstance().GameManagerAudio.clip = _startMatchSFX;
        GameManager_Depricated.GetInstance().GameManagerAudio.Play();
    }
}
