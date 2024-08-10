using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Core.AI
{
    public class UninitializedState : IAIState
    {
        public AIState State => AIState.Uninitialized;

        public void OnStateEnter(Character character, AIState prevState, AIStateMachine stateMachine)
        {

        }

        public void OnStateExit(Character character, AIState newState, AIStateMachine stateMachine)
        { 
        }

        public void UpdateState(Character character, AIStateMachine stateMachine)
        {
            Debug.LogWarning($"UninitializedState Being Updated!!! Check {character.name}'s AIStateMachine.");
        }
    }
}