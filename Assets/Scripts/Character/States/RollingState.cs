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
            Debug.Log($"{prevState} -> RollingState ");

            timeElapsed = 0.0f;
            character.IsRolling = true;
            character.AccelerateToTargetHorizontalSpeed = false; // 구르기 상태에서는 가속/감속 없이 속력을 일시적으로 변경한다.
            character.OverrideMovementVector = true; // 구르기 상태는 입력한 방향으로 강제로 움직인다.
            character.OverridedMovementVector = character.Controller.MovementInput;
            character.RollingCooldownTime = 0.0f;
            character.TargetHorizontalSpeed = character.movementSettings.RollingSpeed;
        }

        public void OnStateExit(Character character, CharacterState newState)
        {
            Debug.Log($"RollingState -> {newState}");

            timeElapsed = 0.0f;
            character.IsRolling = false;
            character.AccelerateToTargetHorizontalSpeed = true; // 가속/감속을 다시 원래대로 되돌린다.
            character.OverrideMovementVector = false; // 인풋 방향대로 움직이도록 되돌린다.
            character.OverridedMovementVector = Vector2.zero;
        }

        public void UpdateState(Character character, CharacterStateMachine stateMachine)
        {
            timeElapsed += Time.deltaTime;
            if(timeElapsed >= character.movementSettings.RollingDuration)
            {
                Debug.Log(timeElapsed);
                if (character.Controller.MovementInput == Vector2.zero)
                {
                    character.HorizontalSpeed = character.TargetHorizontalSpeed = character.movementSettings.WalkSpeed;
                    stateMachine.TransitionToState(CharacterState.Idle);
                }
                else
                {
                    character.HorizontalSpeed = character.TargetHorizontalSpeed = character.movementSettings.WalkSpeed;
                    stateMachine.TransitionToState(CharacterState.GroundMove);
                }
            }
        }
    }
}