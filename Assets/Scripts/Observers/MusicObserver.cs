using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicObserver : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _menuMusic;
    [SerializeField] AudioClip _preMatchMusic;
    [SerializeField] AudioClip _localMatchMusic;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {

        SceneManager.sceneLoaded += PlayMenuMusic;
        SceneManager.sceneLoaded += StopMenuMusic;
        GameManagerUIObserver.onMatchCountdownEnd += PlayLocalMatchMusic;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= PlayMenuMusic;
        SceneManager.sceneLoaded -= StopMenuMusic;
        GameManagerUIObserver.onMatchCountdownEnd -= PlayLocalMatchMusic;
    }
    public void PlayMenuMusic(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "MainMenu")
        {
            _audioSource.clip = _menuMusic;
            _audioSource.Play();
        }
        else
        {
            return;
        }
    }
    public void PlayLocalMatchMusic()
    {
        _audioSource.clip = _localMatchMusic;
        _audioSource.Play();
    }
    public void StopMenuMusic(Scene scene, LoadSceneMode mode)
    {
        _audioSource.Stop();
    }
}
