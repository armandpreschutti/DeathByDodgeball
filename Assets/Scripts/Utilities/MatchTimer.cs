using System;
using UnityEngine;
using TMPro; // For displaying the timer (optional, if you're using TextMeshPro)

public class MatchTimer : MonoBehaviour
{
    [SerializeField] private int startTime = 3; // Set this in the Inspector for initial time in minutes
    [SerializeField] private TMP_Text timerText; // Optional: Attach a TMP_Text UI component for display

    public AudioSource audio;
    private int remainingTimeInSeconds;
    private bool isRunning;
    public static Action onMatchTimeOut;
    

    private void OnEnable()
    {
        MatchInstanceManager.onStartMatch += StartTimer;
        MatchInstanceManager.onStopMatchElements += StopTimer;
    }
    private void OnDisable()
    {
        MatchInstanceManager.onStartMatch -= StartTimer;
        MatchInstanceManager.onStopMatchElements -= StopTimer;
    }

    void Start()
    {
        remainingTimeInSeconds = startTime;
        UpdateTimerDisplay(); // Show the initial time

        //StartTimer();
    }

    public void StartTimer()
    {
        if (!isRunning)
        {
            isRunning = true;
            StartCoroutine(CountdownCoroutine());
        }
    }

    public void StopTimer()
    {
        isRunning = false;
        StopAllCoroutines();
    }

    private System.Collections.IEnumerator CountdownCoroutine()
    {
        while (remainingTimeInSeconds > 0)
        {
            yield return new WaitForSeconds(1);
            remainingTimeInSeconds--;
            UpdateTimerDisplay();
        }

        isRunning = false;
        TimerEnded();
    }

    private void UpdateTimerDisplay()
    {
        int minutes = remainingTimeInSeconds / 60;
        int seconds = remainingTimeInSeconds % 60;
        string formattedTime = string.Format("{0}:{1:00}", minutes, seconds);

        if (timerText != null)
        {
            timerText.text = formattedTime; // Update the UI text
            if(remainingTimeInSeconds < 11)
            {

                timerText.color = Color.red;
            }
            if(remainingTimeInSeconds < 5 && remainingTimeInSeconds >0)
            {
                audio.Play();
            }
        }

        //Debug.Log(formattedTime); // Optional: Log the time to the console
    }

    private void TimerEnded()
    {
        onMatchTimeOut?.Invoke();
        Debug.LogError("Timer has ended!");
        // Add any additional actions here, like triggering an event or stopping gameplay.
    }
}