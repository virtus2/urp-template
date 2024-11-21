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

        public Vector3 GetCurrentVelocity(Character character, KinematicCharacterMotor motor)
        {
            return Vector3.zero;
        }
    }
}