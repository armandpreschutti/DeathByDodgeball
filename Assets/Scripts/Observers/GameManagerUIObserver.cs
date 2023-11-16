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
        GameManager.onStartSceneTransition+= StartSceneTransition;
        GameManager.onEndSceneTransition += EndSceneTransition;
        LocalMatchManager.onMatchCountdown += StartMatchCountdown;
    }
    private void OnDisable()
    {
        GameManager.onStartSceneTransition-= StartSceneTransition;
        GameManager.onEndSceneTransition -= EndSceneTransition;
        LocalMatchManager.onMatchCountdown -= StartMatchCountdown;
    }

    public void StartSceneTransition()
    {
        GameManager.GetInstance().Anim.Play("Start");
    }
    public void EndSceneTransition()
    {
        GameManager.GetInstance().Anim.Play("End");
    }

    public void StartMatchCountdown()
    {
        GameManager.GetInstance().Anim.Play("MatchCountdown");
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
        GameManager.GetInstance().GameManagerAudio.clip = _getReadySFX;
        GameManager.GetInstance().GameManagerAudio.Play();
    }
    public void PlayCountSFX()
    {
        GameManager.GetInstance().GameManagerAudio.clip = _countSFX;
        GameManager.GetInstance().GameManagerAudio.Play();
    }
    public void PlayStartMatchSFX()
    {
        GameManager.GetInstance().GameManagerAudio.clip = _startMatchSFX;
        GameManager.GetInstance().GameManagerAudio.Play();
    }
}
