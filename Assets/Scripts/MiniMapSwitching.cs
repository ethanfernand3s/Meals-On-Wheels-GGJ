using UnityEngine;

public class MiniMapSwitching : MonoBehaviour
{
    public GameObject minimap;
    public GameObject centerMap;

    private bool isMinimapActive = true;

    void Start()
    {
        // Set initial state
        minimap.SetActive(true);
        centerMap.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.M))
        {
            isMinimapActive = !isMinimapActive;

            minimap.SetActive(isMinimapActive);
            centerMap.SetActive(!isMinimapActive);
        }
    }
}