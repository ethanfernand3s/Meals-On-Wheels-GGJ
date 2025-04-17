
using UnityEngine;
 
public class PlayerMovement : MonoBehaviour 
{
    public float moveSpeed;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity = 0.1f;
    CharacterController characterController;
    private Transform _camTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        _camTransform = Camera.main.transform;
    }
 
 
    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal") ;
        float z = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(x, 0f, z).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camTransform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }
        
    }
}