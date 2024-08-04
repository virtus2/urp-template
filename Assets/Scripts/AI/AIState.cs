namespace Core
{
    public enum AIState
    {
        Uninitialized,

        Idle,
        Wandering,
    }

    public interface IAIState
    {
        AIState State { get; }
        void OnStateEnter(Character character, AIState prevState);
        void OnStateExit(Character character, AIState newState);
        void UpdateState(Character character, AIStateMachine stateMachine);
    }
}