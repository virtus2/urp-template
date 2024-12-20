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
            // �̵� �Է��� ���� ��
            if(character.Controller.MovementInputVector.sqrMagnitude > float.Epsilon)
            {
                if(character.Controller.RollPressed && character.CanRoll())
                {
                    stateMachine.TransitionToState(CharacterState.Rolling);
                    return;
                }
                stateMachine.TransitionToState(CharacterState.GroundMove);
            }

            // ���� �Է� �� Attack ���·� ��ȯ�Ѵ�.
            if (character.Controller.AttackPressed)
            {
                stateMachine.TransitionToState(CharacterState.Attack);
                return;
            }
        }

        public Quaternion GetCurrentRotation(Character character, KinematicCharacterMotor motor, float deltaTime)
        {
            return motor.TransientRotation;
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