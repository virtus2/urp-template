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
            Vector2 movementInput = character.Controller.MovementInput;
            character.Controller.MovementVector = Vector3.right * movementInput.x + Vector3.forward * movementInput.y;
            character.Controller.MovementVector *= character.movementSettings.RunningSpeed;

            if(character.Controller.MovementInput.sqrMagnitude <= 0)
            {
                stateMachine.TransitionToState(CharacterState.Idle);
            }

            if(character.Controller.RollPressed && character.CanRoll())
            {
                stateMachine.TransitionToState(CharacterState.Rolling);
            }
        }
    }
}