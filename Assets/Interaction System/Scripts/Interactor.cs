using System.Collections.Generic;
using UnityEngine;

namespace Tools.Interaction
{
    public class Interactor : MonoBehaviour
    {
        private Interactable _currentInteractable;
        private List<Interactable> _detectedInteractables = new();

        private void Update()
        {
            if (_currentInteractable != null && Input.GetKeyDown(KeyCode.E))
            {
                _currentInteractable.Interact(this);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Interactable interactable = collision.GetComponent<Interactable>();
            if (interactable != null && !_detectedInteractables.Contains(interactable))
            {
                _detectedInteractables.Add(interactable);
                UpdateClosestInteractable();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Interactable interactable = collision.GetComponent<Interactable>();
            if (interactable != null && _detectedInteractables.Contains(interactable))
            {
                _detectedInteractables.Remove(interactable);
                if (_currentInteractable == interactable)
                {
                    _currentInteractable.LostFocus();
                    _currentInteractable = null;
                    UpdateClosestInteractable();
                }
            }
        }

        private void UpdateClosestInteractable()
        {
            if (_detectedInteractables.Count == 0)
                return;            

            Interactable closestInteractable = _detectedInteractables[0];
            float closestDistance = Vector3.Distance(transform.position, closestInteractable.transform.position);

            foreach (Interactable interactable in _detectedInteractables)
            {
                float distance = Vector3.Distance(transform.position, interactable.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = interactable;
                }
            }

            if (_currentInteractable != null && _currentInteractable != closestInteractable)
            {
                _currentInteractable.LostFocus();
                _currentInteractable = closestInteractable;
                _currentInteractable.Focus();
            }
            else if (_currentInteractable == null)
            {
                _currentInteractable = closestInteractable;
                _currentInteractable.Focus();
            }
        }
    }
}