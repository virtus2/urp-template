using KinematicCharacterController;
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
            timeElapsed = 0.0f;
            character.IsRolling = true;
            // TODO: 구르기 다시 구현
            // character.AccelerateToTargetHorizontalSpeed = character.RollingSettings.AccelerationToTargetSpeed; // 구르기 상태에서는 가속/감속 없이 속력을 일시적으로 변경한다.
            // character.SetOverrideMovementVector(true, character.Controller.MovementInput); // 구르기 상태는 입력한 방향으로 강제로 움직인다.
            character.RollingCooldownTime = 0.0f;
        }

        public void OnStateExit(Character character, CharacterState newState)
        {
            timeElapsed = 0.0f;
            character.IsRolling = false;
            // character.AccelerateToTargetHorizontalSpeed = true; // 가속/감속을 다시 원래대로 되돌린다.
            // character.SetOverrideMovementVector(false, Vector2.zero); // 인풋 방향대로 움직이도록 되돌린다.
        }

        public void UpdateState(Character character, CharacterStateMachine stateMachine)
        {
            timeElapsed += Time.deltaTime;
            character.Controller.MaxStableMoveSpeed = character.RollingSettings.RollingSpeedCurve.Evaluate(timeElapsed);

            if (timeElapsed >= character.RollingSettings.RollingDuration)
            {
                if (character.Controller.MovementInputVector.sqrMagnitude > float.Epsilon)
                {
                    character.Controller.MaxStableMoveSpeed = character.MovementSettings.WalkSpeed;
                    character.Controller.MaxStableMoveSpeed = character.MovementSettings.WalkSpeed;
                    stateMachine.TransitionToState(CharacterState.Idle);
                }
                else
                {
                    character.Controller.MaxStableMoveSpeed = character.MovementSettings.WalkSpeed;
                    character.Controller.MaxStableMoveSpeed = character.MovementSettings.WalkSpeed;
                    stateMachine.TransitionToState(CharacterState.GroundMove);
                }
            }
        }

        public Vector3 GetCurrentVelocity(Character character, KinematicCharacterMotor motor)
        {
            return Vector3.zero;
        }
    }
}