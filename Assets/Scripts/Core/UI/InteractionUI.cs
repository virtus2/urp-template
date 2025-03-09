using Core.Character.Component;
using UnityEngine;

namespace Core.UI
{
    public class InteractionUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup Widget;

        private CharacterInteractionComponent interactionComponent;

        private void Update()
        {
            if (Player.PlayerInstance.Instance.PlayerCharacter)
            {
                interactionComponent = Player.PlayerInstance.Instance.PlayerCharacter.GetComponent<CharacterInteractionComponent>();
            }

            if (interactionComponent)
            {
                Widget.gameObject.SetActive(interactionComponent.interactables.Count > 0);
            }
        }
    }
}