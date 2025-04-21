using UnityEngine;
using System.IO;

public class MapScreenshot : MonoBehaviour
{
    public Camera mapCamera;
    public int resolutionWidth = 3840;  // e.g., 4K width
    public int resolutionHeight = 2160; // e.g., 4K height
    public string outputFileName = "MapRender.png";

    [ContextMenu("Capture Map Screenshot")]
    public void CaptureScreenshot()
    {
        RenderTexture rt = new RenderTexture(resolutionWidth, resolutionHeight, 24);
        mapCamera.targetTexture = rt;

        Texture2D image = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.RGB24, false);
        mapCamera.Render();

        RenderTexture.active = rt;
        image.ReadPixels(new Rect(0, 0, resolutionWidth, resolutionHeight), 0, 0);
        image.Apply();

        mapCamera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(rt);

        byte[] bytes = image.EncodeToPNG();
        string path = Path.Combine(Application.dataPath, outputFileName);
        File.WriteAllBytes(path, bytes);

        Debug.Log("Map Screenshot saved to: " + path);
    }
}