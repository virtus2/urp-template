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
            if(character.Controller.MovementInput != Vector2.zero)
            {
                if(character.Controller.RollPressed && character.CanRoll())
                {
                    stateMachine.TransitionToState(CharacterState.Rolling);
                    return;
                }
                stateMachine.TransitionToState(CharacterState.GroundMove);
            }
        }
    }
}