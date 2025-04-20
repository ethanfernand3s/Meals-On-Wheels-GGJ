using System;
using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    public static Pickup Instance;
    public GameObject player;
    public Transform holdPos;
    
    public float throwForce = 500f; //force at which the object is thrown at
    private float rotationSensitivity = 1f; //how fast/slow the object is rotated in relation to mouse movement
    public GameObject heldObj; //object which we pick up
    private Rigidbody heldObjRb; //rigidbody of object we pick up
    private bool canDrop = true; //this is needed so we don't throw/drop object when rotating the object
    private int LayerNumber; //layer index
    
    private Coroutine FloatingAnimCoroutine;
    
    void Start()
    {
        if(Instance == null)
            Instance = this;
        LayerNumber = LayerMask.NameToLayer("holdLayer");
        
        FloatingAnimCoroutine = StartCoroutine(PickupFloatingAnimation());
    }

    private IEnumerator PickupFloatingAnimation()
    {
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        
        while (heldObj == null)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, .75f + Mathf.Sin(Time.time) * 0.5f, gameObject.transform.position.z);
            
            yield return null;
        }
    }
    public void Interact(RaycastHit hit)
    {
        if (heldObj == null) //if currently not holding anything
        {
            //make sure pickup tag is attached
            if (hit.transform.gameObject.tag == "canPickUp")
            {
                //pass in object hit into the PickUpObject function
                PickUpObject(hit.transform.gameObject);
            }
            StopCoroutine(FloatingAnimCoroutine);
            FloatingAnimCoroutine = null;
        }
        else
        {
            if (canDrop)
            {
                StopClipping(); //prevents object from clipping through walls
                DropObject();
            }
        }
    }
    void Update()
    {
        if (heldObj != null) //if player is holding object
        {
            MoveObject(); //keep object position at holdPos
            RotateObject();
            if (Input.GetKeyDown(KeyCode.Mouse0) && canDrop && Cursor.lockState == CursorLockMode.Locked)
            {
                StopClipping();
                ThrowObject();
            }
        }
        else
        {
            if (gameObject.GetComponent<Rigidbody>().linearVelocity == Vector3.zero)
            {
                FloatingAnimCoroutine = StartCoroutine(PickupFloatingAnimation());
            }
        }
    }
    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>()) //make sure the object has a RigidBody
        {
            heldObj = pickUpObj; //assign heldObj to the object that was hit by the raycast (no longer == null)
            heldObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody
            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = holdPos.transform; //parent object to holdposition
            heldObj.layer = LayerNumber; //change the object layer to the holdLayer
            //make sure object doesnt collide with player, it can cause weird bugs
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
    }
    void DropObject()
    {
        //re-enable collision with player
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 3; //object assigned back to default layer
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null; //unparent object
        heldObj = null; //undefine game object
    }
    void MoveObject()
    {
        //keep object position the same as the holdPosition position
        heldObj.transform.position = holdPos.transform.position;
    }
    void RotateObject()
    {
        if (Input.GetKey(KeyCode.R))
        {
            canDrop = false; //make sure throwing can't occur during rotating

            Vector3 rightRotation = Vector3.right;
            heldObj.transform.Rotate(rightRotation * rotationSensitivity);
        }
        else
        {
            canDrop = true;
        }
    }
    void ThrowObject()
    {
        //same as drop function, but add force to object before undefining it
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 3;
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObjRb.AddForce(transform.forward * throwForce);
        heldObj = null;
    }
    void StopClipping() //function only called when dropping/throwing
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); //distance from holdPos to the camera
        //have to use RaycastAll as object blocks raycast in center screen
        //RaycastAll returns array of all colliders hit within the cliprange
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        //if the array length is greater than 1, meaning it has hit more than just the object we are carrying
        if (hits.Length > 1)
        {
            //change object position to camera position 
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
            //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
        }
    }
}
