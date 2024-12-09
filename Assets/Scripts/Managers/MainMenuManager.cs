using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MainMenuManager : MonoBehaviour
{
    public PlayableDirector director;
    public PlayableAsset intro;
    public static Action onGameReady;

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
        director.playableAsset = intro;
        director.Play();
    }

}
