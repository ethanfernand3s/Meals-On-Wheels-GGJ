using UnityEditor;
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

    public void QuitButton()
    {
#if UNITY_EDITOR
        // Stop play mode if in Editor
        EditorApplication.isPlaying = false;
#else
        // Quit the application if in a build
        Application.Quit();
#endif
    }
}
