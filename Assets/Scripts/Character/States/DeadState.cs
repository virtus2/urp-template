using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace Core
{
    public class DeadState : ICharacterState
    {
        public CharacterState State => CharacterState.Dead;

        public void OnStateEnter(Character character, CharacterState prevState)
        {
            character.IsDead = true;
            character.InputEnabled = false;
            character.Controller.SetCollisions(false);
            // TODO: ���׵�?
        }

        public void OnStateExit(Character character, CharacterState newState)
        {
            // TODO: ���� ���°� Dead�� �ƴϸ� ������ ����
            if(newState != CharacterState.Dead)
            {
                character.IsDead = false;
                character.InputEnabled = true;
                character.Controller.SetCollisions(true);
            }
        }

        public void UpdateState(Character character, CharacterStateMachine stateMachine)
        {
        }

        public Vector3 GetCurrentVelocity(Character character, KinematicCharacterMotor motor)
        {
            return Vector3.zero;
        }
    }
}