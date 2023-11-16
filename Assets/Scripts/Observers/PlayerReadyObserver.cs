using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerReadyObserver : MonoBehaviour
{
    public static event Action<PlayerManager, bool> onPrimePlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogWarning($"{collision.name} is primed gameplay");
        if (collision.tag == "Player")
        {
            onPrimePlayer?.Invoke(collision.GetComponent<PlayerManager>(), true);
            collision.GetComponent<PlayerManager>().readyPrompt.SetActive(true);
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
            onPrimePlayer?.Invoke(collision.GetComponent<PlayerManager>(), false);
            collision.GetComponent<PlayerManager>().readyPrompt.SetActive(false);
        }
        else
        {
            return;
        }
    }
}

