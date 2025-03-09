using UnityEngine;

namespace Core
{
    // TODO: Input System Interactions�� ��ü�� �� �ִ°��� Ȯ��
    // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.10/manual/Interactions.html
    public enum EInteractType
    {
        Tap, // �� �� ������
        Hold, // �� ������
    }

    public abstract class InteractableComponent : MonoBehaviour
    {
        public EInteractType InteractionType;
        public float HoldDuration = 0f;
        public bool ShowWidget = true;
        public bool IsInteractable = true;
        public abstract void BeginInteract();
        public abstract void Interact();
        public abstract void EndInteract();
    }
}
