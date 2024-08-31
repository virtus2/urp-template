namespace Core
{
    /// <summary>
    /// Interact ��ư�� ������ ��ȣ�ۿ��� �� �ִ� �������̽�
    /// </summary>
    public interface IInteractable
    {
        public bool NeedToShowWidget();
        public bool IsInteractable();
        public void BeginInteract();
        public void Interact();
        public void EndInteract();
    }
}
