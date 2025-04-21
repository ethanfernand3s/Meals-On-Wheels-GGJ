using UnityEngine;
using TMPro;

public class TimeText : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Assign in Inspector
    public GameObject finishedGameScreen;
    private float _startTime;
    private float _endTime;
    private bool _timerRunning = false;

    void Start()
    {
        timerText.text = "00:00:00";
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
        if (finishedGameScreen != null)
        {
            finishedGameScreen.SetActive(true); 
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (_timerRunning)
        {
            _endTime = Time.time;
            _timerRunning = false;

            if (timerText != null)
                timerText.text = FormatTime(_endTime - _startTime);
        }
    }

    public float GetScoreTime()
    {
        return _timerRunning ? Time.time - _startTime : _endTime - _startTime;
    }

    private string FormatTime(float time)
    {
        int hours = Mathf.FloorToInt(time / 3600f);
        int minutes = Mathf.FloorToInt((time % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }
}