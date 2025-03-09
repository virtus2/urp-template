using Core.Character.Component;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Player.UI
{
    public class InteractionWidget : MonoBehaviour
    {
        [SerializeField] private CanvasGroup Widget;

        private CharacterInteractionComponent interactionComponent;

        private void Update()
        {
            if (PlayerInstance.Instance.PlayerCharacter)
            {
                interactionComponent = PlayerInstance.Instance.PlayerCharacter.GetComponent<CharacterInteractionComponent>();
            }

            if (interactionComponent)
            {
                Widget.gameObject.SetActive(interactionComponent.interactables.Count > 0);
            }
        }
    }
}