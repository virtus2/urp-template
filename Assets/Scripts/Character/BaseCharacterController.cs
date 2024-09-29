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

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        public Vector2 LastMovementInput;
        public Vector2 MovementInput; // 컨트롤러의 움직이는 방향 입력
        public Vector3 MovementVector; // 최종적으로 캐릭터가 움직이는 방향
        public Vector3 LookVector; 
        public float VerticalVelocity;
        public bool HasMovementInput;
        public bool RollPressed;
        public bool AttackPressed;
        public bool RunPressed;
        public bool InteractPressed;

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
            if (!character) return;
            if (!characterController) return;

            characterController.Move(MovementVector * Time.deltaTime);
        }

        public void SetMovementInput(Vector2 input)
        {
            bool inputChanged = input.sqrMagnitude > 0.0f;

            if (HasMovementInput && !inputChanged)
            {
                LastMovementInput = MovementInput;
            }

            MovementInput = input;
            HasMovementInput = inputChanged;
        }

        public void SetDetectCollisions(bool detectCollisions)
        {
            characterController.detectCollisions = detectCollisions;
        }
    }
}