using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerExitObserver : MonoBehaviour
{
    public static event Action<PlayerManager, bool> onPlayerExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            onPlayerExit?.Invoke(collision.GetComponent<PlayerManager>(), true);
            collision.GetComponent<PlayerManager>().exitPrompt.SetActive(true);
        }
        else
        {
            return;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.LogWarning($"{collision.name} is no longer primed for gameplay");
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerManager>().exitPrompt.SetActive(false);
        }
        else
        {
            return;
        }
    }
}
