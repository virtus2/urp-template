using KinematicCharacterController;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "SO_CharacterState_Attack", menuName = "Scriptable Objects/Character/State/Attack")]
    public class CharacterStateSO_Attack : CharacterStateSO
    {
        public float AttackDuration = 1f;
        public AnimationCurve AttackVelocityCurve;
        public float AttackVelocityMultiplier = 1f;

        public override CharacterState CreateInstance()
        {
            CharacterState_Attack state = new CharacterState_Attack();
            state.AttackDuration = AttackDuration;
            state.AttackVelocityCurve = AttackVelocityCurve;
            state.AttackVelocityMultiplier = AttackVelocityMultiplier;
            return state;
        }
    }

    public class CharacterState_Attack : CharacterState
    {
        public float AttackDuration = 1f;
        public AnimationCurve AttackVelocityCurve;
        public float AttackVelocityMultiplier = 1f;

        private float elapsedTime = 0.0f;

        public override void OnStateEnter(Character character, ECharacterState prevState)
        {
            elapsedTime = 0.0f;

            character.IsAttacking = true;
        }

        public override void OnStateExit(Character character, ECharacterState newState)
        {
            character.IsAttacking = false;
        }

        public override void UpdateState(Character character, CharacterStateMachine stateMachine)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > AttackDuration)
            {
                stateMachine.TransitionToState(ECharacterState.Idle);
                return;
            }

        }

        public override void UpdateVelocity(Character character, KinematicCharacterMotor motor, ref Vector3 currentVelocity, float deltaTime)
        {
            currentVelocity = currentVelocity + AttackVelocityCurve.Evaluate(elapsedTime) * AttackVelocityMultiplier * motor.CharacterForward;
        }

        public override void UpdateRotation(Character character, KinematicCharacterMotor motor, ref Quaternion currentRotation, float deltaTime)
        {
         
        }
    }
}