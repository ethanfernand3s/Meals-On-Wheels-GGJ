using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class RenderingMiniMap : MonoBehaviour
{
    private bool previousFog;

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