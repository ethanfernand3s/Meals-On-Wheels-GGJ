using UnityEngine;
using TMPro;

public class TimeText : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Assign the live timer (top corner)
    public GameObject finishedGameScreen; // Assign the COMPLETE panel

    private float _startTime;
    private float _endTime;
    private bool _timerRunning = false;

    void Start()
    {
        timerText.text = "00:00:00";
        StartTimer();
    }

    void Update()
    {
        if (_timerRunning && timerText != null)
        {
            timerText.text = FormatTime(Time.time - _startTime);
        }
    }

    public void StartTimer()
    {
        _startTime = Time.time;
        _timerRunning = true;
    }

    public void StopTimer()
    {
        if (_timerRunning)
        {
            _endTime = Time.time;
            _timerRunning = false;

            float totalTime = _endTime - _startTime;

            if (timerText != null)
                timerText.text = FormatTime(totalTime);

            // Show end screen
            if (finishedGameScreen != null)
            {
                finishedGameScreen.SetActive(true);

                // Get the component and set final time
                FinishedGameScreen screenScript = finishedGameScreen.GetComponent<FinishedGameScreen>();
                if (screenScript != null)
                {
                    screenScript.SetFinalTime();
                }

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public string GetFormattedFinalTime()
    {
        float finalTime = _timerRunning ? Time.time - _startTime : _endTime - _startTime;
        return FormatTime(finalTime);
    }

    private string FormatTime(float time)
    {
        int hours = Mathf.FloorToInt(time / 3600f);
        int minutes = Mathf.FloorToInt((time % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }
}
