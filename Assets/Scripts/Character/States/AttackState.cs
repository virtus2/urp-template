using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace Core
{
    public class AttackState : ICharacterState
    {
        public CharacterState State => CharacterState.Attack;

        private float timeElapsed = 0.0f;

        public void OnStateEnter(Character character, CharacterState prevState)
        {
            timeElapsed = 0.0f;

            character.IsAttacking = true;
            character.SetOverrideMovementVector(true, Vector2.zero);
        }

        public void OnStateExit(Character character, CharacterState newState)
        {
            character.IsAttacking = false;
            character.SetOverrideMovementVector(false, Vector2.zero);
        }

        public void UpdateState(Character character, CharacterStateMachine stateMachine)
        {
            timeElapsed += Time.deltaTime;
            if(timeElapsed > 1.0f)
            {
                stateMachine.TransitionToState(CharacterState.Idle);
                return;
            }
        }
    }
}