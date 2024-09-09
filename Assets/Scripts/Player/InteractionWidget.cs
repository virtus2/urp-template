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
        [SerializeField] private Ease Ease;
        [SerializeField] private float Time;

        private CharacterInteractionComponent interactionComponent;

        private void OnEnable()
        {
            Player.Instance.OnPlayerCharacterSpawned += SetInteractionComponent;
            Widget.gameObject.SetActive(false);
            Hide();
        }

        private void SetInteractionComponent(Character PlayerCharacter)
        {
            interactionComponent = PlayerCharacter.GetComponent<CharacterInteractionComponent>();
            if (interactionComponent)
            {
                interactionComponent.OnInteractableObjectChanged += OnInteractableObjectChanged;
            }
        }

        private void OnDisable()
        {
            Player.Instance.OnPlayerCharacterSpawned -= SetInteractionComponent;
            if (interactionComponent)
            {
                interactionComponent.OnInteractableObjectChanged -= OnInteractableObjectChanged;
            }
        }

        private void OnInteractableObjectChanged(IInteractable interactable)
        {
            if(interactable != null && interactable.NeedToShowWidget())
            {
                Widget.gameObject.SetActive(true);
                Show();
            }
            else
            {
                Hide();
            }
        }

        // TODO: ��ư, Ȧ���ư ���� �������� ��ȣ�ۿ� Ÿ�Կ� ���ؼ� UI ǥ��
        private void Show()
        {
            Widget.DOFade(1, Time).SetEase(Ease);
            Widget.transform.DOScale(1, Time).SetEase(Ease);
        }

        private void Hide()
        {
            Widget.DOFade(0, 0.1f).SetEase(Ease);
            Widget.transform.DOScale(0, 0.1f).SetEase(Ease);
        }
    }
}