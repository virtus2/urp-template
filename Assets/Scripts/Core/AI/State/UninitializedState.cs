using UnityEngine;

namespace Core.AI.State
{
    public class UninitializedState : IAIState
    {
        public AIState State => AIState.Uninitialized;

        public void OnStateEnter(Core.Character.BaseCharacter character, AIState prevState, AIStateMachine stateMachine)
        {

        }

        public void OnStateExit(Core.Character.BaseCharacter character, AIState newState, AIStateMachine stateMachine)
        { 
        }

        public void UpdateState(Core.Character.BaseCharacter character, AIStateMachine stateMachine)
        {
            Debug.LogWarning($"UninitializedState Being Updated!!! Check {character.name}'s AIStateMachine.");
        }
    }
}