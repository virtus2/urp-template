using UnityEngine.AI;

namespace Core
{
    public enum AIState
    {
        Uninitialized,

        Idle,
        Wandering,
        Chase,
    }

    public interface IAIState
    {
        AIState State { get; }
        void OnStateEnter(Character character, AIState prevState, AIStateMachine stateMachine);
        void OnStateExit(Character character, AIState newState, AIStateMachine stateMachine);
        void UpdateState(Character character, AIStateMachine stateMachine);
    }
}