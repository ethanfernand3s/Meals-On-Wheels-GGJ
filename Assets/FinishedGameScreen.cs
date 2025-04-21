using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // If you want to load scenes later

public class FinishedGameScreen : MonoBehaviour
{
    public TextMeshProUGUI finalTimeText; // Assign in Inspector
    public GameObject cutsceneObject; // Optional: a GameObject to enable as a cutscene

    // Called by TimeText script
    public void SetFinalTime(float totalTime)
    {
        int hours = Mathf.FloorToInt(totalTime / 3600f);
        int minutes = Mathf.FloorToInt((totalTime % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(totalTime % 60f);
        finalTimeText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    // Assign this to a button in UI to play the cutscene
    public void PlayCutscene()
    {
        if (cutsceneObject != null)
        {
            cutsceneObject.SetActive(true); // Or trigger an animation/camera/etc.
        }

        // Optional: load scene after cutscene
        // SceneManager.LoadScene("NextScene");
    }
}