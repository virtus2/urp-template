using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Core
{
    public class InteractionWidget : MonoBehaviour
    {
        [SerializeField] private CanvasGroup Widget;

        private CharacterInteractionComponent interactionComponent;

        private void Update()
        {
            if (Player.Instance.PlayerCharacter)
            {
                interactionComponent = Player.Instance.PlayerCharacter.GetComponent<CharacterInteractionComponent>();
            }

            if (interactionComponent)
            {
                Widget.gameObject.SetActive(interactionComponent.interactables.Count > 0);
            }
        }
    }
}