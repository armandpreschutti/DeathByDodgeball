using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenuManager : MonoBehaviour
{
    public static Action onGameReady;

    public void OnGameStart()
    {
        onGameReady?.Invoke();
    }
}
