using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI References")]
    public GameObject gameOverUI;
    public GameObject pauseMenuUI;

    private bool isGameOver = false;
    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            FindFirstObjectByType<AudioManager>().Play("BackgroundMusic");
            DontDestroyOnLoad(gameObject); // Optional
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            TogglePause();
        }
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Time.timeScale = 0f;

        FindFirstObjectByType<AudioManager>().Play("GameWon");
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;

        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(isPaused);

        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused;
        
        if (isPaused)
        {
            FindFirstObjectByType<AudioManager>().Play("ScreenClose");
        }
        else
        {
            FindFirstObjectByType<AudioManager>().Play("ScreenPopUp");
        }
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}