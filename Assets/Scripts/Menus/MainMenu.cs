using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public FadeManager fadeManager;
    public void playButton()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        fadeManager.FadeToScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void settingsButton()
    {
        
    }
}
