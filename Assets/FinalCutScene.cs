using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalCutScene : MonoBehaviour
{
    [Header("Slide Settings")]
    public List<Sprite> slides;
    public float fadeDuration = 1f;

    [Header("Audio")]
    public AudioSource musicSource;
    public AudioClip backgroundMusic;
    public AudioSource voiceSource;
    public AudioClip narrationClip; // ← Single voiceover audio

    [Header("UI References")]
    public Image slideshowPanel;
    public Image finalImage;
    public Button mainMenuButton;

    private int currentSlide = 0;
    private CanvasGroup canvasGroup;
    private bool isTransitioning = false;

    private void Start()
    {
        // Add CanvasGroup for fade control
        canvasGroup = slideshowPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = slideshowPanel.gameObject.AddComponent<CanvasGroup>();

        // Hide final image and menu button
        finalImage.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);

        // Play background music
        if (backgroundMusic != null && musicSource != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
        }

        // Play voice narration (once)
        if (narrationClip != null && voiceSource != null)
        {
            voiceSource.clip = narrationClip;
            voiceSource.loop = false;
            voiceSource.Play();
        }

        StartCoroutine(ShowSlide(currentSlide));
    }

    private void Update()
    {
        if (!isTransitioning && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
        {
            StartCoroutine(NextSlide());
        }
    }

    private IEnumerator ShowSlide(int index)
    {
        isTransitioning = true;

        slideshowPanel.sprite = slides[index];
        yield return StartCoroutine(FadeIn());

        isTransitioning = false;
    }

    private IEnumerator NextSlide()
    {
        isTransitioning = true;

        yield return StartCoroutine(FadeOut());

        currentSlide++;

        if (currentSlide < slides.Count)
        {
            yield return StartCoroutine(ShowSlide(currentSlide));
        }
        else
        {
            yield return StartCoroutine(FadeToFinal());
        }
    }

    private IEnumerator FadeIn()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOut()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }

    private IEnumerator FadeToFinal()
    {
        slideshowPanel.gameObject.SetActive(false);

        // Final image fade-in
        finalImage.gameObject.SetActive(true);
        Color imageColor = finalImage.color;
        imageColor.a = 0f;
        finalImage.color = imageColor;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            float alpha = timer / fadeDuration;
            finalImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, alpha);
            timer += Time.deltaTime;
            yield return null;
        }
        finalImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, 1f);

        // Fade in main menu button
        mainMenuButton.gameObject.SetActive(true);
        CanvasGroup buttonGroup = mainMenuButton.GetComponent<CanvasGroup>();
        if (buttonGroup == null)
            buttonGroup = mainMenuButton.gameObject.AddComponent<CanvasGroup>();

        buttonGroup.alpha = 0f;
        timer = 0f;
        while (timer < fadeDuration)
        {
            buttonGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        buttonGroup.alpha = 1f;

        mainMenuButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);// Replace with your scene name
        });

        isTransitioning = false;
    }
}
