namespace Core
{
    // Interact ��ư�� ������ ��ȣ�ۿ��� �� �ִ� �������̽�
    public interface IInteractable
    {
        public bool IsInteractable();
        public void BeginInteract();
        public void Interact();
        public void EndInteract();
    }
}
