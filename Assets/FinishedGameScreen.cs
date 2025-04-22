using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishedGameScreen : MonoBehaviour
{
    public TextMeshProUGUI finalTimeText; // Assign the "00'00'00" text
    public Button nextSceneButton;

    private TimeText timeText;

    void Start()
    {
        // Find the TimeText object (modern Unity API)
        timeText = Object.FindFirstObjectByType<TimeText>();
        SetFinalTime();
    }

    public void SetFinalTime()
    {
        if (finalTimeText != null && timeText != null)
        {
            string formattedTime = timeText.GetFormattedFinalTime();
            finalTimeText.text = formattedTime;
            Debug.Log("Final Time Set To: " + formattedTime);
        }
        else
        {
            Debug.LogWarning("Missing references: TimeText or finalTimeText");
        }
    }

    public void GoToNextScene()
    {
        SceneManager.LoadScene(2); // Loads scene with index 2
    }
}