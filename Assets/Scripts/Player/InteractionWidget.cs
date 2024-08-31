using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class InteractionWidget : MonoBehaviour
    {
        [SerializeField]
        private GameObject widget;

        private CharacterInteractionComponent InteractionComponent;

        private void Awake()
        {
            Player.Instance.OnPlayerCharacterSpawned += SetInteractionComponent;
        }

        private void SetInteractionComponent(Character PlayerCharacter)
        {
            InteractionComponent = Player.Instance.PlayerCharacter.GetComponent<CharacterInteractionComponent>();
            if (InteractionComponent)
            {
                InteractionComponent.OnInteractableObjectChanged += Show;
            }
        }

        private void OnDisable()
        {
            Player.Instance.OnPlayerCharacterSpawned -= SetInteractionComponent;
            if (InteractionComponent)
            {
                InteractionComponent.OnInteractableObjectChanged -= Show;
            }
        }

        private void Show(IInteractable interactable)
        {
            if(interactable != null)
            {
                if (interactable.NeedToShowWidget())
                {
                    widget.SetActive(interactable != null);
                }
            }
            else
            {
                widget.SetActive(false);
            }
        }
    }
}