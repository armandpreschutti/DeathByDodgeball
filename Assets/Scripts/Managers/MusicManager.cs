using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioClip menuMusic;
    public AudioClip matchInstanceMusic;
    public AudioClip postGameMusic;
    public AudioSource audioSource;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += PlayMenuMusic;
        //TitleMenuManager.onGameReady += PlayMenuMusic;
        PlayerSelectionManager.onLeavingPlayerSelection += StopMenuMusic;
        MatchInstanceManager.onStartMatch += PlayMatchInstanceMusic;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= PlayMenuMusic;
        //TitleMenuManager.onGameReady -= PlayMenuMusic;
        PlayerSelectionManager.onLeavingPlayerSelection -= StopMenuMusic;
        MatchInstanceManager.onStartMatch -= PlayMatchInstanceMusic;
    }

    public void PlayMusicTrack(AudioClip clip)
    {
        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlayMenuMusic(Scene scene, LoadSceneMode mode)
    { 
        if(scene.name == "MainMenu")
        {
            audioSource.clip = menuMusic;
            audioSource.Play();
        }
        if(scene.name == "MatchInstance")
        {
            audioSource.Stop();
        }
        else if(scene.name == "PostMatch")
        {
            audioSource.clip = postGameMusic;
            audioSource.Play();
        }

    }

    public void StopMenuMusic()
    {
        audioSource.Stop();
    }

    public void PlayMatchInstanceMusic()
    {
        audioSource.clip = matchInstanceMusic;
        audioSource.Play();
    }
}
