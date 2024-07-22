using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public abstract class Controller : MonoBehaviour
    {
        public Vector3 Velocity => characterController.velocity;
        public abstract Vector3 GetMovementVector3();

        protected Character character;
        protected CharacterController characterController; // Unity's CharacterController Component
        protected CharacterStateMachine stateMachine;

        protected Vector3 movementVector;

        public virtual void SetCharacter(Character character)
        {
            this.character = character;
            characterController = character.GetComponent<CharacterController>();
            stateMachine = character.GetComponent<CharacterStateMachine>();
        }

        public virtual void Update()
        {
            movementVector = GetMovementVector3();

            if(movementVector.sqrMagnitude > 0)
            {
                if(stateMachine.CurrentState == CharacterState.Idle)
                {
                    stateMachine.TransitionToState(CharacterState.GroundMove);
                }
            }
        }

        private void FixedUpdate()
        {
            characterController.Move(movementVector * character.movementSettings.RunningSpeed * Time.deltaTime);
        }
    }
}