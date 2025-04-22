using System;
using DefaultNamespace;
using UnityEngine;

public class DoorsBehavior : MonoBehaviour, IInteractable
{
    public Animator animator;
    private bool _isDoorOpen = false;
    private Collider _collider;

    private void Start()
    {
        _isDoorOpen = false;
        _collider = gameObject.GetComponent<Collider>();
    }

    private void Update()
    {
        if (_isDoorOpen && transform.parent.GetComponent<Rigidbody>().linearVelocity.magnitude > 0.1f)
        {
            animator.Play("DoorsClose");
            _isDoorOpen = false;
            _collider.isTrigger = false;
        }
    }

    public void Interact(RaycastHit hit)
    {
        if (!_isDoorOpen)
        {
            animator.Play("DoorsOpen");
            
            _collider.isTrigger = true;
            _isDoorOpen = true;
        }
        else
        {
            animator.Play("DoorsClose");
            _collider.isTrigger = false;
            _isDoorOpen = false;
        }
    }
}
