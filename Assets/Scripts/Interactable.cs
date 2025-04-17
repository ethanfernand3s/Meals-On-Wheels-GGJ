using UnityEngine;

namespace DefaultNamespace
{
    public interface IInteractable
    {
        void Interact(RaycastHit hit);
    }
}