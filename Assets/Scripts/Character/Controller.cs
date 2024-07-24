using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public abstract class Controller : MonoBehaviour
    {
        public Vector3 Velocity => characterController.velocity;

        protected Character character;
        protected CharacterController characterController; // Unity's CharacterController Component
        protected CharacterStateMachine stateMachine;

        public Vector3 MovementVector;

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        public Vector2 MovementInput;
        public bool RollPressed;

        public virtual void SetCharacter(Character character)
        {
            this.character = character;
            characterController = character.GetComponent<CharacterController>();
            stateMachine = character.GetComponent<CharacterStateMachine>();
        }

        protected virtual void Update()
        {
        }

        protected void FixedUpdate()
        {
            characterController.Move(MovementVector * Time.deltaTime);
        }
    }
}