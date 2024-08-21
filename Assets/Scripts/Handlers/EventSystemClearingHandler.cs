using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class EventSystemClearingHandler : MonoBehaviour
{
    EventSystem eventSystem;
    InputSystemUIInputModule inputSystemUIInputModule;
    public GameObject firstSelectedObject;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += ClearAllEventSystems;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= ClearAllEventSystems;
    }

    public void ClearAllEventSystems(Scene scene, LoadSceneMode mode)
    {
        eventSystem = GetComponent<EventSystem>();
        eventSystem.SetSelectedGameObject(firstSelectedObject);
        inputSystemUIInputModule = GetComponent<InputSystemUIInputModule>();
        eventSystem.UpdateModules();
        
        
        EventSystem[] eventSystems;
        eventSystems = FindObjectsOfType<EventSystem>();
        for (int i = 0; i < eventSystems.Length; i++)
        {
            if (eventSystems[i] != eventSystem)
            {
                Destroy(eventSystems[i]);
            }
        }
    }
}
