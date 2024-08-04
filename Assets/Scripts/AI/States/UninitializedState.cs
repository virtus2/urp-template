using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Core.AI
{
    public class UninitializedState : IAIState
    {
        public AIState State => AIState.Uninitialized;

        public void OnStateEnter(Character character, AIState prevState)
        {

        }

        public void OnStateExit(Character character, AIState newState)
        { 
        }

        public void UpdateState(Character character, AIStateMachine stateMachine)
        {
            Debug.LogWarning($"UninitializedState Being Updated!!! Check {character.name}'s AIStateMachine.");
        }
    }
}