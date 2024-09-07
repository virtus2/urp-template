namespace Core
{
    // TODO: Input System Interactions로 대체할 수 있는건지 확인
    // https://docs.unity3d.com/Packages/com.unity.inputsystem@1.10/manual/Interactions.html
    public enum EInteractType
    {
        Tap, // 한 번 누르기
        Hold, // 꾹 누르기
    }
    /// <summary>
    /// Interact 버튼을 눌러서 상호작용할 수 있는 인터페이스
    /// </summary>
    public interface IInteractable
    {
        public EInteractType Type { get; }
        public float HoldDuration { get; }
        public bool NeedToShowWidget();
        public bool IsInteractable();
        public void BeginInteract();
        public void Interact();
        public void EndInteract();
    }
}
