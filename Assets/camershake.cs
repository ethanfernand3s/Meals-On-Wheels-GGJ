using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Shake Settings")]
    public float shakeIntensity = 0.1f;     // How strong the shake is
    public float shakeSpeed = 1f;           // How fast the noise moves
    public Vector3 shakeDirection = new Vector3(1, 1, 0); // Which axes to shake on

    private Vector3 initialPosition;
    private float seed;

    void Start()
    {
        initialPosition = transform.localPosition;
        seed = Random.Range(0f, 100f); // Unique seed for variation
    }

    void Update()
    {
        float time = Time.time * shakeSpeed;

        float offsetX = (Mathf.PerlinNoise(seed, time) - 0.5f) * 2f;
        float offsetY = (Mathf.PerlinNoise(seed + 1f, time) - 0.5f) * 2f;
        float offsetZ = (Mathf.PerlinNoise(seed + 2f, time) - 0.5f) * 2f;

        Vector3 shakeOffset = new Vector3(offsetX, offsetY, offsetZ);
        shakeOffset = Vector3.Scale(shakeOffset, shakeDirection.normalized) * shakeIntensity;

        transform.localPosition = initialPosition + shakeOffset;
    }
}
