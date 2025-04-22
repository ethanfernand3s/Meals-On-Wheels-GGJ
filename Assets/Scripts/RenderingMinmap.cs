using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RenderingMiniMap : MonoBehaviour
{
    private bool previousFog;
    private Color previousBackground;

    private Camera cam;

    void OnEnable()
    {
        cam = GetComponent<Camera>();
        // Ensure the camera background is transparent
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0, 0, 0, 0); // Transparent
    }

    void OnPreRender()
    {
        previousFog = RenderSettings.fog;
        RenderSettings.fog = false;
    }

    void OnPostRender()
    {
        RenderSettings.fog = previousFog;
    }
}