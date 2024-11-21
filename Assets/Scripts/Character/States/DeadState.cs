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
            // TODO: 래그돌?
        }

        public void OnStateExit(Character character, CharacterState newState)
        {
            // TODO: 다음 상태가 Dead가 아니면 무조건 원복
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