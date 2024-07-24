using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

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

            if (character.Controller.MovementInput == Vector2.zero)
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