using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Windows;

namespace Core
{
    public class AttackState : ICharacterState
    {
        public CharacterState State => CharacterState.Attack;
        public float AttackDuration = 0.2f;
        private float timeElapsed = 0.0f;

        public void OnStateEnter(Character character, CharacterState prevState)
        {
            timeElapsed = 0.0f;

            character.IsAttacking = true;
        }

        public void OnStateExit(Character character, CharacterState newState)
        {
            character.IsAttacking = false;
        }

        public void UpdateState(Character character, CharacterStateMachine stateMachine)
        {
            timeElapsed += Time.deltaTime;
            if(timeElapsed > AttackDuration)
            {
                stateMachine.TransitionToState(CharacterState.Idle);
                return;
            }
        }

        public Quaternion GetCurrentRotation(Character character, KinematicCharacterMotor motor, float deltaTime)
        {
            return motor.TransientRotation;
        }

        public void UpdateVelocity(Character character, KinematicCharacterMotor motor, ref Vector3 Velocity, float deltaTime)
        {
            Velocity += character.Controller.AttackVelocityCurve.Evaluate(timeElapsed) * 
                character.Controller.AttackVelocityMultiplier *
                motor.CharacterForward;
        }
    }
}