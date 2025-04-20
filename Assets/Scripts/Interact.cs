using System;
using DefaultNamespace;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactRange = 3f;
    public KeyCode interactKey = KeyCode.E;
    public LayerMask interactLayer;

    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * interactRange, Color.red);

        // Use the interactLayer in the Raycast
        if (!Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer)) return;

        GameObject hitObject = hit.collider.gameObject;

        IInteractable interactable = hitObject.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactable.Interact(hit);
        }   
        else
        {
            Debug.LogWarning($"'{hitObject.name}' Can't Interact With Object Since There Is No Interactable Interface");
        }
    }
}
