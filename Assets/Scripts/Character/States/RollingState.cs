using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class RollingState : ICharacterState
    {
        public CharacterState State => CharacterState.Rolling;

        private float timeElapsed = 0.0f;

        public void OnStateEnter(Character character, CharacterState prevState)
        {
            Debug.Log("RollingState OnStateEnter");

            timeElapsed = 0.0f;
            character.IsRolling = true;
            character.RollingCooldownTime = 0.0f;

            Vector2 movementInput = character.Controller.MovementInput;
            character.Controller.MovementVector = Vector3.right * movementInput.x + Vector3.forward * movementInput.y;
            character.Controller.MovementVector *= character.movementSettings.RollingSpeed;
        }

        public void OnStateExit(Character character, CharacterState newState)
        {
            Debug.Log("RollingState OnStateExit");

            timeElapsed = 0.0f;
            character.IsRolling = false;
        }

        public void UpdateState(Character character, CharacterStateMachine stateMachine)
        {
            timeElapsed += Time.deltaTime;
            if(timeElapsed >= character.movementSettings.RollingDuration)
            {
                if (character.Controller.MovementInput.sqrMagnitude <= 0)
                {
                    character.Controller.MovementVector = Vector3.zero;
                    stateMachine.TransitionToState(CharacterState.Idle);

                }
                else
                {
                    stateMachine.TransitionToState(CharacterState.GroundMove);
                }
            }
        }
    }
}