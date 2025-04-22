using UnityEngine;

public class EndlessRoad : MonoBehaviour
{
    [Header("Road Setup")]
    public GameObject roadPrefab;                // Road tile prefab
    public int roadCount = 3;                    // Number of road tiles
    public float segmentLength = 20f;            // Distance between tiles

    [Tooltip("Scroll direction in world space (e.g. 1, 0, 0 for +X)")]
    public Vector3 scrollDirection = Vector3.right;

    [Tooltip("Euler angle offset applied to each segment")]
    public Vector3 rotationOffset = Vector3.zero;

    [Tooltip("Optional override for the starting position of the road")]
    public Transform customSpawnOrigin;

    [Header("Scroll Settings")]
    public float scrollSpeed = 10f;

    private GameObject[] roadSegments;
    private Vector3 origin;

    void Start()
    {
        if (roadPrefab == null)
        {
            Debug.LogError("❌ Road Prefab is not assigned.");
            return;
        }

        segmentLength = Mathf.Max(0.01f, segmentLength);
        Vector3 normalizedDirection = scrollDirection.normalized;

        origin = customSpawnOrigin != null ? customSpawnOrigin.position : transform.position;

        roadSegments = new GameObject[roadCount];
        for (int i = 0; i < roadCount; i++)
        {
            Vector3 spawnPos = origin + normalizedDirection * segmentLength * i;
            Quaternion spawnRot = Quaternion.Euler(rotationOffset);
            roadSegments[i] = Instantiate(roadPrefab, spawnPos, spawnRot);
        }
    }

    void Update()
    {
        Vector3 normalizedDirection = scrollDirection.normalized;

        foreach (GameObject segment in roadSegments)
        {
            segment.transform.Translate(normalizedDirection * scrollSpeed * Time.deltaTime, Space.World);
        }

        for (int i = 0; i < roadSegments.Length; i++)
        {
            float distanceFromOrigin = Vector3.Dot(roadSegments[i].transform.position - origin, normalizedDirection);

            if (distanceFromOrigin > segmentLength * (roadCount - 1))
            {
                int backIndex = GetFurthestBehindIndex(normalizedDirection);
                Vector3 newPos = roadSegments[backIndex].transform.position - normalizedDirection * segmentLength;
                roadSegments[i].transform.position = newPos;
                roadSegments[i].transform.rotation = Quaternion.Euler(rotationOffset);
            }
        }
    }

    int GetFurthestBehindIndex(Vector3 direction)
    {
        int index = 0;
        float minProjection = float.MaxValue;

        for (int i = 0; i < roadSegments.Length; i++)
        {
            float projection = Vector3.Dot(roadSegments[i].transform.position, direction);
            if (projection < minProjection)
            {
                minProjection = projection;
                index = i;
            }
        }

        return index;
    }
}
