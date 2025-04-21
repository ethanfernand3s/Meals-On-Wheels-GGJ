using UnityEngine;
public class OutOfBoundsVolume : MonoBehaviour
{
    public Vector3 boxCenter = new Vector3(0, 0, 0);
    public Vector3 boxSize = new Vector3(100, 100, 100);
    public Transform safePosition;

    public bool IsOutsideBounds(Vector3 position)
    {
        Bounds bounds = new Bounds(boxCenter, boxSize);
        return !bounds.Contains(position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject rootObj = other.transform.root.gameObject;

        if (rootObj.CompareTag("Player"))
        {
            rootObj.transform.position = safePosition.position;
        }
    }
}
