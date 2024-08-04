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
            character.AccelerateToTargetHorizontalSpeed = true;
            character.TargetHorizontalSpeed = character.Controller.SprintPressed ? character.MovementSettings.SprintSpeed : character.MovementSettings.WalkSpeed;
        }

        public void OnStateExit(Character character, CharacterState newState)
        {
        }

        public void UpdateState(Character character, CharacterStateMachine stateMachine)
        {
            // GroundMove 상태에서 이동 입력이 없으면 Idle 상태로 전환한다.
            if (character.Controller.MovementInput == Vector2.zero)
            {
                character.TargetHorizontalSpeed = 0.0f;
                stateMachine.TransitionToState(CharacterState.Idle);
                return;
            }

            // 구르기 입력 시 Rolling 상태로 전환한다.
            if(character.Controller.RollPressed && character.CanRoll())
            {
                stateMachine.TransitionToState(CharacterState.Rolling);
                return;
            }
            character.TargetHorizontalSpeed = character.Controller.SprintPressed ? character.MovementSettings.SprintSpeed : character.MovementSettings.WalkSpeed;
        }
    }
}