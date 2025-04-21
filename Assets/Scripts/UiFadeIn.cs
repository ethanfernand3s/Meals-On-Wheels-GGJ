using UnityEngine;
using UnityEngine.UI;

public class UiFadeOut : MonoBehaviour
{
    [SerializeField] private Image fadeImage;     // Drag the Image here in Inspector
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        if (fadeImage == null)
        {
            Debug.LogError("Fade Image not assigned in the Inspector.");
            enabled = false;
        }
    }

    private void Start()
    {
        // Start fully opaque and unclickable
        Color color = fadeImage.color;
        color.a = 1f;
        fadeImage.color = color;
        fadeImage.raycastTarget = false;

        StartCoroutine(FadeOut());
    }

    private System.Collections.IEnumerator FadeOut()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(1f - (timer / fadeDuration));
            Color color = fadeImage.color;
            color.a = alpha;
            fadeImage.color = color;
            yield return null;
        }

        // Keep it invisible and non-interactable
        fadeImage.raycastTarget = false;
    }
}