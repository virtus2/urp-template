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
            if(character.Controller.MovementInput != Vector2.zero)
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
    }
}