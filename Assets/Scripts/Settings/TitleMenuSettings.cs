using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleMenuSettings : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _startButton;
    [SerializeField] Color _flashColor;

    private void OnEnable()
    {
       // GameManager.onStartSceneTransition += DisableStartButton;
    }

    private void OnDisable()
    {
        ///GameManager.onStartSceneTransition -= DisableStartButton;
    }

    public void SwitchToMainMenu()
    {
        _startButton.color = _flashColor;
        GameManager_Depricated.GetInstance().SwitchScene("MainMenu");
    }
   /* public void DisableStartButton()
    {
        _startButton.SetActive(false);
    }*/
    IEnumerator ButtonFlash(TextMeshProUGUI button)
    {
        yield return new WaitForSeconds(.10f);
        button.color = Color.white;
        yield return new WaitForSeconds(.10f);
        button.color = _flashColor;
        yield return new WaitForSeconds(.10f);
        button.color = Color.white;
        yield return new WaitForSeconds(.10f);
        button.color = _flashColor;
    }

}
