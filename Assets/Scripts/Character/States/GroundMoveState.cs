using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class GroundMoveState : ICharacterState
    {
        public CharacterState State => CharacterState.GroundMove;

        public void OnStateEnter(Character character, CharacterState prevState)
        {
            Debug.Log("GroundMoveState OnStateEnter");
        }

        public void OnStateExit(Character character, CharacterState newState)
        {
            Debug.Log("GroundMoveState OnStateExit");
        }

        public void UpdateState(Character character, CharacterStateMachine stateMachine)
        {
            if (character.Velocity.sqrMagnitude <= 0)
            {
                stateMachine.TransitionToState(CharacterState.Idle);
                return;
            }
        }
    }
}