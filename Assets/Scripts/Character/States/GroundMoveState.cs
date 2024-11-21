using KinematicCharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace Core
{
    public class GroundMoveState : ICharacterState
    {
        public CharacterState State => CharacterState.GroundMove;

        public void OnStateEnter(Character character, CharacterState prevState)
        {
            character.Controller.MaxStableMoveSpeed = character.Controller.RunPressed ?
                character.MovementSettings.RunSpeed : character.MovementSettings.WalkSpeed;
        }

        public void OnStateExit(Character character, CharacterState newState)
        {
        }

        public void UpdateState(Character character, CharacterStateMachine stateMachine)
        {
            // GroundMove 상태에서 이동 입력이 없으면 Idle 상태로 전환한다.
            if (character.Controller.MovementInputVector.sqrMagnitude <= float.Epsilon)
            {
                character.Controller.MaxStableMoveSpeed = 0.0f;
                stateMachine.TransitionToState(CharacterState.Idle);
                return;
            }

            // 구르기 입력 시 Rolling 상태로 전환한다.
            if (character.Controller.RollPressed && character.CanRoll())
            {
                stateMachine.TransitionToState(CharacterState.Rolling);
                return;
            }

            // 공격 입력 시 Attack 상태로 전환한다.
            if (character.Controller.AttackPressed)
            {
                stateMachine.TransitionToState(CharacterState.Attack);
                return;
            }

            character.Controller.MaxStableMoveSpeed = character.Controller.RunPressed ?
                character.MovementSettings.RunSpeed : character.MovementSettings.WalkSpeed;
        }

        public Vector3 GetCurrentVelocity(Character character, KinematicCharacterMotor motor)
        {
            Vector3 currentVelocity = character.Controller.Velocity;
            float deltaTime = Time.deltaTime;

            // Ground movement
            if (motor.GroundingStatus.IsStableOnGround)
            {
                float currentVelocityMagnitude = currentVelocity.magnitude;

                Vector3 effectiveGroundNormal = motor.GroundingStatus.GroundNormal;

                // Reorient velocity on slope
                currentVelocity = motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) * currentVelocityMagnitude;

                // Calculate target velocity
                Vector3 inputRight = Vector3.Cross(character.Controller.MovementInputVector, motor.CharacterUp);
                Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized * character.Controller.MovementInputVector.magnitude;
                Vector3 targetMovementVelocity = reorientedInput * character.Controller.MaxStableMoveSpeed;

                // Smooth movement Velocity
                currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-character.Controller.StableMovementSharpness * deltaTime));
            }
            // Air movement
            else
            {
                // Add move input
                if (character.Controller.MovementInputVector.sqrMagnitude > 0f)
                {
                    Vector3 addedVelocity = character.Controller.MovementInputVector * character.Controller.AirAccelerationSpeed * deltaTime;

                    Vector3 currentVelocityOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, motor.CharacterUp);

                    // Limit air velocity from inputs
                    if (currentVelocityOnInputsPlane.magnitude < character.Controller.MaxAirMoveSpeed)
                    {
                        // clamp addedVel to make total vel not exceed max vel on inputs plane
                        Vector3 newTotal = Vector3.ClampMagnitude(currentVelocityOnInputsPlane + addedVelocity, character.Controller.MaxAirMoveSpeed);
                        addedVelocity = newTotal - currentVelocityOnInputsPlane;
                    }
                    else
                    {
                        // Make sure added vel doesn't go in the direction of the already-exceeding velocity
                        if (Vector3.Dot(currentVelocityOnInputsPlane, addedVelocity) > 0f)
                        {
                            addedVelocity = Vector3.ProjectOnPlane(addedVelocity, currentVelocityOnInputsPlane.normalized);
                        }
                    }

                    // Prevent air-climbing sloped walls
                    if (motor.GroundingStatus.FoundAnyGround)
                    {
                        if (Vector3.Dot(currentVelocity + addedVelocity, addedVelocity) > 0f)
                        {
                            Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(motor.CharacterUp, motor.GroundingStatus.GroundNormal), motor.CharacterUp).normalized;
                            addedVelocity = Vector3.ProjectOnPlane(addedVelocity, perpenticularObstructionNormal);
                        }
                    }

                    // Apply added velocity
                    currentVelocity += addedVelocity;
                }

                // Gravity
                currentVelocity += character.Controller.Gravity * deltaTime;

                // Drag
                currentVelocity *= (1f / (1f + (character.Controller.Drag * deltaTime)));
            }

            return currentVelocity;
        }
    }
}