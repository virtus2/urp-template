using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class UninitializedState : ICharacterState
    {
        public CharacterState State => CharacterState.Uninitialized;

        public void OnStateEnter(Character character, CharacterState prevState)
        {
            
        }

        public void OnStateExit(Character character, CharacterState newState)
        {
            
        }

        public void UpdateState(Character character, CharacterStateMachine stateMachine)
        {
            Debug.LogWarning($"UninitializedState Being Updated!!! Check {character.name}'s CharacterStateMachine.");
        }
    }
}