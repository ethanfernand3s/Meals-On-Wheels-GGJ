using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public Renderer fadeQuadRenderer; // Assign your Quad's Renderer
    public float fadeDuration = 1f;

    private Material fadeMaterial;
    private Color fadeColor;

    private void Start()
    {
        // Clone the material instance so we don't overwrite the shared material
        fadeMaterial = fadeQuadRenderer.material;
        fadeColor = fadeMaterial.color;
        fadeColor.a = 1f;
        fadeMaterial.color = fadeColor;

        StartCoroutine(FadeIn());
    }

    public void FadeToScene(int sceneIndex)
    {
        StartCoroutine(FadeOut(sceneIndex));
    }

    public IEnumerator FadeIn()
    {
        float t = fadeDuration;
        while (t > 0)
        {
            t -= Time.deltaTime;
            fadeColor.a = t / fadeDuration;
            fadeMaterial.color = fadeColor;
            yield return null;
        }

        fadeColor.a = 0f;
        fadeMaterial.color = fadeColor;

        // Optional: Disable or destroy the quad
        fadeQuadRenderer.gameObject.SetActive(false);
        // Destroy(fadeQuadRenderer.gameObject); // Only if you never need it again
    }

    private IEnumerator FadeOut(int sceneIndex)
    {
        fadeQuadRenderer.gameObject.SetActive(true);
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeColor.a = t / fadeDuration;
            fadeMaterial.color = fadeColor;
            yield return null;
        }

        fadeColor.a = 1f;
        fadeMaterial.color = fadeColor;

        SceneManager.LoadScene(sceneIndex);
    }
}
