using UnityEngine;

public class ChaseCamera : MonoBehaviour
{
    public Transform target;                         // Car target
    public Vector3 offset = new Vector3(0f, 2f, -6f); // Offset behind car
    public float followSmoothness = 10f;             // How smoothly camera follows position
    public float lookSensitivity = 2f;               // Mouse rotation speed
    public float yawClamp = 45f;                     // Max left/right look
    public float pitchMin = -10f;
    public float pitchMax = 15f;

    private float yaw = 0f;
    private float pitch = 5f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (!target) return;

        // Escape to unlock cursor (optional)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Mouse input
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        yaw = Mathf.Clamp(yaw + mouseX, -yawClamp, yawClamp);
        pitch = Mathf.Clamp(pitch - mouseY, pitchMin, pitchMax);

        // Rotation based on car direction + mouse input
        Quaternion baseRotation = Quaternion.Euler(0f, target.eulerAngles.y, 0f);
        Quaternion orbitRotation = Quaternion.Euler(pitch, yaw, 0f);
        Quaternion finalRotation = baseRotation * orbitRotation;

        // Position camera with smooth follow
        Vector3 desiredPosition = target.position + finalRotation * offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * followSmoothness);

        // Look at the car's upper center
        Vector3 lookPoint = target.position + Vector3.up * 1.5f;
        transform.rotation = Quaternion.LookRotation(lookPoint - transform.position);
    }
}
