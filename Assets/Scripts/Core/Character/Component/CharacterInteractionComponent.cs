using System.Collections.Generic;
using UnityEngine;

namespace Core.Character.Component
{
    [RequireComponent(typeof(BaseCharacter))]
    public class CharacterInteractionComponent : MonoBehaviour
    {
        public List<InteractableComponent> interactables;
        public bool CanInteractWithObjects = true;

        private BaseCharacter character;

        private void Awake()
        {
            character = GetComponent<BaseCharacter>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var component = other.GetComponent<InteractableComponent>();
            if (component != null)
            {
                interactables.Add(component);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var component = other.GetComponent<InteractableComponent>();
            if (component != null)
            {
                interactables.Remove(component);
            }
        }

        private void Update()
        {
            if (interactables.Count > 0)
            {
                for(int i=interactables.Count-1; i>=0; i--)
                {
                    var obj = interactables[i];
                    if (obj.IsInteractable && CanInteractWithObjects)
                    {
                        if (character.Controller.InteractPressed)
                        {
                            obj.BeginInteract();
                            obj.Interact();
                            obj.EndInteract();
                            return;
                        }
                    }
                }
            }
        }
    }
}