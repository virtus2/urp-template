using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class IdleState : ICharacterState
    {
        public CharacterState State => CharacterState.Idle;

        public void OnStateEnter(Character character, CharacterState prevState)
        {
        }

        public void OnStateExit(Character character, CharacterState newState)
        {
        }

        public void UpdateState(Character character, CharacterStateMachine stateMachine)
        {
            // 이동 입력이 있을 때
            if(character.Controller.MovementInputVector.sqrMagnitude > float.Epsilon)
            {
                if(character.Controller.RollPressed && character.CanRoll())
                {
                    stateMachine.TransitionToState(CharacterState.Rolling);
                    return;
                }
                stateMachine.TransitionToState(CharacterState.GroundMove);
            }

            // 공격 입력 시 Attack 상태로 전환한다.
            if (character.Controller.AttackPressed)
            {
                stateMachine.TransitionToState(CharacterState.Attack);
                return;
            }
        }

        public Quaternion GetCurrentRotation(Character character, KinematicCharacterMotor motor, float deltaTime)
        {
            Quaternion currentRotation = motor.TransientRotation;
            if (character.Controller.LookInputVector.sqrMagnitude > 0f && character.Controller.OrientationSharpness > 0f)
            {
                // Smoothly interpolate from current to target look direction
                Vector3 smoothedLookInputDirection = Vector3.Slerp(motor.CharacterForward, character.Controller.LookInputVector, 1 - Mathf.Exp(-character.Controller.OrientationSharpness * deltaTime)).normalized;

                // Set the current rotation (which will be used by the KinematicCharacterMotor)
                currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, motor.CharacterUp);
            }
            return currentRotation;
        }

        public void UpdateVelocity(Character character, KinematicCharacterMotor motor, ref Vector3 currentVelocity, float deltaTime)
        {
            if (motor.GroundingStatus.IsStableOnGround)
            {
                currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, 1f - Mathf.Exp(-character.Controller.StableMovementSharpness * deltaTime));
            }
            else
            {
                // Gravity
                currentVelocity += character.Controller.Gravity * deltaTime;
            }
        }
    }
}