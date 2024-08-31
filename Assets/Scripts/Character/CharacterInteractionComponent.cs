using System;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Character))]
    public class CharacterInteractionComponent : CharacterBaseComponent
    {
        public IInteractable CurrentInteractableObject; 
        public bool CanInteractWithObjects = true;
        public Action<IInteractable> OnInteractableObjectChanged;

        private void OnTriggerEnter(Collider other)
        {
            var interactableObject = other.GetComponent<IInteractable>();
            if (interactableObject != null)
            {
                if (interactableObject.IsInteractable() && CanInteractWithObjects)
                {
                    CurrentInteractableObject = interactableObject;
                    OnInteractableObjectChanged?.Invoke(interactableObject);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var interactableObject = other.GetComponent<IInteractable>();
            if (interactableObject != null)
            {
                if(interactableObject == CurrentInteractableObject)
                {
                    CurrentInteractableObject = null;
                    OnInteractableObjectChanged?.Invoke(null);
                }
            }
        }

        private void Update()
        {
            if (CurrentInteractableObject == null || character == null) 
            { 
                return;
            }

            if(CurrentInteractableObject.IsInteractable() && CanInteractWithObjects && character.Controller.InteractPressed)
            {
                CurrentInteractableObject.BeginInteract();
                CurrentInteractableObject.Interact();
                CurrentInteractableObject.EndInteract();
            }
        }
    }
}