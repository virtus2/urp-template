namespace Core
{
    /// <summary>
    /// Interact 버튼을 눌러서 상호작용할 수 있는 인터페이스
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
