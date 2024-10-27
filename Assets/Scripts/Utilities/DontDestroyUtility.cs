using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyUtility : MonoBehaviour
{
    public string killScene;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += DisableDontDestroy;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= DisableDontDestroy;
    }
    public void EnableDontDestroy()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void DisableDontDestroy(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == killScene)
        {
            Destroy(gameObject);
        }
    }
}
