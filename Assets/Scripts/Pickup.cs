using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    public GameObject player;
    public Transform holdPos;
    public FoodType foodType;

    public float throwForce = 500f;
    private float rotationSensitivity = 1f;
    private GameObject heldObj;
    private Rigidbody heldObjRb;
    private bool canDrop = true;
    private int LayerNumber;

    private Coroutine FloatingAnimCoroutine;
    [SerializeField] public Animator playerAnimator;

    private float startY;
    private bool isAttachedToTrunk = false;

    void Start()
    {
        LayerNumber = LayerMask.NameToLayer("holdLayer");
        startY = transform.position.y;
        FloatingAnimCoroutine = StartCoroutine(PickupFloatingAnimation());
    }

    private IEnumerator PickupFloatingAnimation()
    {
        while (heldObj == null && !isAttachedToTrunk)
        {
            float floatOffset = 0.25f * Mathf.Sin(Time.time * 2f);
            transform.position = new Vector3(transform.position.x, startY + floatOffset, transform.position.z);
            yield return null;
        }
    }

    public void Interact(RaycastHit hit)
    {
        if (heldObj == null)
        {
            if (hit.transform.gameObject.CompareTag("canPickUp"))
            {
                PickUpObject(hit.transform.gameObject);
                playerAnimator.SetBool("isHolding", true);
                PlayerStats.instance.holdingObj = true;
                if (FloatingAnimCoroutine != null)
                {
                    StopCoroutine(FloatingAnimCoroutine);
                    FloatingAnimCoroutine = null;
                }
            }
        }
        else
        {
            if (canDrop)
            {
                StopClipping();
                DropObject();
            }
        }
    }

    void Update()
    {
        if (heldObj != null)
        {
            MoveObject();
            RotateObject();

            if (Input.GetKeyDown(KeyCode.Mouse0) && canDrop && Cursor.lockState == CursorLockMode.Locked)
            {
                StopClipping();
                ThrowObject();
            }
        }
        else
        {
            if (!isAttachedToTrunk && GetComponent<Rigidbody>().linearVelocity == Vector3.zero)
            {
                startY = transform.position.y;
                if (FloatingAnimCoroutine == null)
                    FloatingAnimCoroutine = StartCoroutine(PickupFloatingAnimation());
            }
        }
    }

    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.TryGetComponent(out Rigidbody rb))
        {
            heldObj = pickUpObj;
            heldObjRb = rb;
            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = holdPos.transform;
            heldObj.layer = LayerNumber;
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);

            // Reset trunk attachment
            Pickup pickupComponent = heldObj.GetComponent<Pickup>();
            if (pickupComponent != null)
            {
                pickupComponent.isAttachedToTrunk = false;
            }
        }
    }

    void DropObject()
    {
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 3;
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;

        Pickup droppedPickup = heldObj.GetComponent<Pickup>();
        if (droppedPickup != null && droppedPickup.IsTouchingTrunk())
        {
            droppedPickup.AttachToTrunk();
        }

        heldObj = null;
        playerAnimator.SetBool("isHolding", false);
        FloatingAnimCoroutine = StartCoroutine(PickupFloatingAnimation());

        PlayerStats.instance.holdingObj = false;
    }

    void ThrowObject()
    {
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 3;
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;

        heldObjRb.AddForce(Camera.main.transform.forward * throwForce);

        Pickup thrownPickup = heldObj.GetComponent<Pickup>();
        if (thrownPickup != null && thrownPickup.IsTouchingTrunk())
        {
            thrownPickup.AttachToTrunk();
        }

        heldObj = null;
        playerAnimator.SetBool("isHolding", false);
        FloatingAnimCoroutine = StartCoroutine(PickupFloatingAnimation());
        
        PlayerStats.instance.holdingObj = false;
    }

    void MoveObject()
    {
        heldObj.transform.position = holdPos.transform.position;
    }

    void RotateObject()
    {
        if (Input.GetKey(KeyCode.R))
        {
            canDrop = false;
            Vector3 rightRotation = Vector3.right;
            heldObj.transform.Rotate(rightRotation * rotationSensitivity);
        }
        else
        {
            canDrop = true;
        }
    }

    void StopClipping()
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position);
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);

        if (hits.Length > 1)
        {
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f);
        }
    }

    public bool IsTouchingTrunk()
    {
        Collider[] hits = Physics.OverlapBox(transform.position, transform.localScale * 0.5f, Quaternion.identity);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Trunk"))
                return true;
        }
        return false;
    }

    public void AttachToTrunk()
    {
        isAttachedToTrunk = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

        Collider[] hits = Physics.OverlapBox(transform.position, transform.localScale * 0.5f, Quaternion.identity);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Trunk"))
            {
                transform.SetParent(hit.transform, true); // Keep current world position
                break;
            }
        }

        if (FloatingAnimCoroutine != null)
        {
            StopCoroutine(FloatingAnimCoroutine);
            FloatingAnimCoroutine = null;
        }
    }
}
