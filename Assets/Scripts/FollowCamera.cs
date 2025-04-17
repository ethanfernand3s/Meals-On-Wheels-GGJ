using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Camera _cam;

    void Start()
    {
        _cam = Camera.main;
        if (_cam == null)
            Debug.LogError("No MainCamera tagged in scene!");
    }

    void Update()
    {
        if (_cam == null) return;

        // Get camera’s yaw (eulerAngles.y)
        float camYaw = _cam.transform.eulerAngles.y;

        // Build a new rotation: keep player's current X/Z, only swap in camera Y
        Vector3 e = transform.eulerAngles;
        e.y = camYaw;
        transform.eulerAngles = e;
    }
}
