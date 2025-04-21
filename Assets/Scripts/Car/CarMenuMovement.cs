using UnityEngine;

public class CarMenuMovement : MonoBehaviour
{
    [System.Serializable]
    public struct CarWheelTransforms
    {
        public Transform gfxTransform;
    }

    public CarWheelTransforms[] wheelTransforms;

    void Update()
    {
        foreach (var wheel in wheelTransforms)
        {
            if (wheel.gfxTransform != null)
            {
                wheel.gfxTransform.Rotate(Vector3.up, 600f * Time.deltaTime);
            }
        }
    }
}