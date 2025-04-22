using UnityEngine;

[RequireComponent(typeof(Collider))]
public class NpcMapStatus : MonoBehaviour
{
    [Tooltip("This GameObject will be set active when the player steps in.")]
    public GameObject objectToEnable;

    private bool bIsCompleted = false;
    void Start()
    {
        // Ensure collider is a trigger
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && objectToEnable != null && !bIsCompleted)
        {
            bIsCompleted = true;
            objectToEnable.SetActive(true);
        }
    }
}
