using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalCutScene : MonoBehaviour
{
    [Header("Slide Settings")]
    public List<Sprite> slides;

    [Header("UI References")]
    public Image slideshowImage;
    public Image finalImage;
    public Button mainMenuButton;
    public Sprite finalSlideImage;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;

    private int currentSlide = 0;
    private CanvasGroup canvasGroup;
    private bool isTransitioning = false;

    private void Start()
    {
        canvasGroup = slideshowImage.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = slideshowImage.gameObject.AddComponent<CanvasGroup>();

        if (slides != null && slides.Count > 0)
        {
            slideshowImage.sprite = slides[0];
        }

        finalImage.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
        canvasGroup.alpha = 1f;
    }

    private void Update()
    {
        if (!isTransitioning && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(AdvanceSlide());
        }
    }

    private IEnumerator AdvanceSlide()
    {
        isTransitioning = true;

        // Fade out
        yield return StartCoroutine(FadeOut());

        currentSlide++;

        if (currentSlide < slides.Count)
        {
            slideshowImage.sprite = slides[currentSlide];
            yield return StartCoroutine(FadeIn());
            isTransitioning = false;
        }
        else
        {
            slideshowImage.gameObject.SetActive(false);

            if (finalImage != null && finalSlideImage != null)
            {
                finalImage.sprite = finalSlideImage;
                finalImage.color = new Color(1, 1, 1, 0);
                finalImage.gameObject.SetActive(true);

                float timer = 0f;
                while (timer < fadeDuration)
                {
                    float alpha = timer / fadeDuration;
                    finalImage.color = new Color(1, 1, 1, alpha);
                    timer += Time.deltaTime;
                    yield return null;
                }
                finalImage.color = Color.white;
            }

            mainMenuButton.gameObject.SetActive(true);
        }
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

    public void ExitToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
