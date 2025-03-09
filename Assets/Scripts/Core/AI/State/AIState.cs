using UnityEngine.AI;

namespace Core.AI.State
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
        void OnStateEnter(Core.Character.BaseCharacter character, AIState prevState, AIStateMachine stateMachine);
        void OnStateExit(Core.Character.BaseCharacter character, AIState newState, AIStateMachine stateMachine);
        void UpdateState(Core.Character.BaseCharacter character, AIStateMachine stateMachine);
    }
}