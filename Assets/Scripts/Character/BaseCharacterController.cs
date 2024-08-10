using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public abstract class BaseCharacterController : MonoBehaviour
    {
        public Vector3 Velocity => characterController.velocity;
        public float Radius => characterController.radius;

        public Vector3 MovementVector;
        public Vector2 MovementInput; // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        public float VerticalVelocity;
        public bool RollPressed;
        public bool SprintPressed;

        protected Character character;
        protected CharacterController characterController; // Unity's CharacterController Component
        protected CharacterStateMachine stateMachine;

        public virtual void SetCharacter(Character character)
        {
            this.character = character;
            
            characterController = character.GetComponent<CharacterController>();
            if(!characterController)
            {
                Debug.LogWarning($"{character.name}에 유니티 CharacterController 컴포넌트가 없습니다.");
            }

            stateMachine = character.GetComponent<CharacterStateMachine>();
            if(!stateMachine)
            {
                Debug.LogWarning($"{character.name}에 CharacterStateMachine 컴포넌트가 없습니다.");
            }
        }

        protected virtual void Update()
        {

        }

        protected void FixedUpdate()
        {
            // TODO: 점프나 사다리타기 등 수직 속도에 대해서 처리할것이 꽤 많다...
            characterController.Move(MovementVector * Time.deltaTime);
        }
    }
}