using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform player;                 // The player's transform to follow
    public Vector3 offset = new Vector3(0, 50, 0); // Offset above the player for the minimap view
    public Vector3 centerPosition = new Vector3(0, 50, 0); // Fixed map center view
    
    private bool isCentered = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.M))
        {
            isCentered = !isCentered; // Toggle centering mode
        }

        if (isCentered)
        {
            // Follow the player directly
            transform.position = player.position + offset;
        }
        else
        {
            // Stay near the fixed center but adjust based on player movement
            Vector3 adjustedPosition = centerPosition + (player.position - centerPosition);
            transform.position = new Vector3(adjustedPosition.x, offset.y, adjustedPosition.z);
        }
    }
}