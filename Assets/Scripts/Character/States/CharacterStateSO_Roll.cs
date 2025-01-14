using KinematicCharacterController;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "SO_CharacterState_Roll", menuName = "Scriptable Objects/Character/State/Roll")]
    public class CharacterStateSO_Roll : CharacterStateSO
    {
        public override CharacterState CreateInstance()
        {
            return new CharacterState_Roll();
        }
    }

    public class CharacterState_Roll : CharacterState
    {
        // TODO: 구르기 다시 구현


        private float elapsedTime = 0.0f;
        public override void OnStateEnter(Character character, ECharacterState prevState)
        {
            elapsedTime = 0.0f;
            character.IsRolling = true;
            // character.AccelerateToTargetHorizontalSpeed = character.RollingSettings.AccelerationToTargetSpeed; // 구르기 상태에서는 가속/감속 없이 속력을 일시적으로 변경한다.
            // character.SetOverrideMovementVector(true, character.Controller.MovementInput); // 구르기 상태는 입력한 방향으로 강제로 움직인다.
            character.RollingCooldownTime = 0.0f;
        }

        public override void OnStateExit(Character character, ECharacterState newState)
        {
            elapsedTime = 0.0f;
            character.IsRolling = false;
            // character.AccelerateToTargetHorizontalSpeed = true; // 가속/감속을 다시 원래대로 되돌린다.
            // character.SetOverrideMovementVector(false, Vector2.zero); // 인풋 방향대로 움직이도록 되돌린다.
        }

        public override void UpdateState(Character character, CharacterStateMachine stateMachine)
        {
            elapsedTime += Time.deltaTime;
            character.Controller.MaxStableMoveSpeed = character.RollingSettings.RollingSpeedCurve.Evaluate(elapsedTime);

            if (elapsedTime >= character.RollingSettings.RollingDuration)
            {
                if (character.Controller.MovementInputVector.sqrMagnitude > float.Epsilon)
                {
                    character.Controller.MaxStableMoveSpeed = character.MovementSettings.WalkSpeed;
                    character.Controller.MaxStableMoveSpeed = character.MovementSettings.WalkSpeed;
                    stateMachine.TransitionToState(ECharacterState.Idle);
                }
                else
                {
                    character.Controller.MaxStableMoveSpeed = character.MovementSettings.WalkSpeed;
                    character.Controller.MaxStableMoveSpeed = character.MovementSettings.WalkSpeed;
                    stateMachine.TransitionToState(ECharacterState.GroundMove);
                }
            }
        }

        public override void UpdateVelocity(Character character, KinematicCharacterMotor motor, ref Vector3 currentVelocity, float deltaTime)
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

        public override void UpdateRotation(Character character, KinematicCharacterMotor motor, ref Quaternion currentRotation, float deltaTime)
        {
            if (character.Controller.LookInputVector.sqrMagnitude > 0f && character.Controller.OrientationSharpness > 0f)
            {
                // Smoothly interpolate from current to target look direction
                Vector3 smoothedLookInputDirection = Vector3.Slerp(motor.CharacterForward, character.Controller.LookInputVector, 1 - Mathf.Exp(-character.Controller.OrientationSharpness * deltaTime)).normalized;

                // Set the current rotation (which will be used by the KinematicCharacterMotor)
                currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, motor.CharacterUp);
            }
        }
    }
}