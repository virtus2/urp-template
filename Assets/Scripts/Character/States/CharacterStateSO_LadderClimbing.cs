using KinematicCharacterController;
using KinematicCharacterController.Walkthrough.ClimbingLadders;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "SO_CharacterState_LadderClimbing", menuName = "Scriptable Objects/Character/State/Ladder Climbing")]
    public class CharacterStateSO_LadderClimbing : CharacterStateSO
    {
        public override CharacterState CreateInstance()
        {
            return new CharacterState_LadderClimbing();
        }
    }

    public class CharacterState_LadderClimbing : CharacterState
    {
        private Vector3 anchoringPosition;
        private Quaternion anchoringRotation;

        private float elapsedTime = 0f;
        private float anchoringDuration_ClimbingOnTop = 2.667f;
        private float anchoringDuration_ClimbingOnBottom = 1.167f;
        private float deanchoringDuration_ClimbingOffTop = 2.667f;
        private float deanchoringDuration_ClimbingOffBottom = 1.0f;
        


        public override void OnStateEnter(Character character, ECharacterState prevState)
        {
            character.Controller.Motor.SetMovementCollisionsSolvingActivation(false);
            character.Controller.Motor.SetGroundSolvingActivation(false);
            character.LadderClimbingStage = ELadderClimbingStage.Anchoring;
            // character.CurrentClimbingLadder is set by StateTransitionSO_Interact
            elapsedTime = 0f;

            // Store the target position and rotation to snap to
            anchoringPosition = character.CurrentClimbingLadder.ClosestPointOnLadderSegment(
                character.Controller.Motor.TransientPosition, out float ladderSegment);
            anchoringRotation = character.CurrentClimbingLadder.transform.rotation;

            if (ladderSegment < 0f)
            {
                character.IsLadderClimbingOnBottom = true;

            }
            else if(ladderSegment > 0.05f)
            {
                character.IsLadderClimbingOnTop = true;
            }
        }

        public override void OnStateExit(Character character, ECharacterState newState)
        {
            character.Controller.Motor.SetMovementCollisionsSolvingActivation(true);
            character.Controller.Motor.SetGroundSolvingActivation(true);
            character.LadderClimbingStage = ELadderClimbingStage.None;
            character.CurrentClimbingLadder = null;
            character.IsLadderClimbing = false;
            character.IsLadderClimbingOnTop = false;
            character.IsLadderClimbingOnBottom = false;
            character.IsLadderClimbingOffTop = false;
            character.IsLadderClimbingOffBottom = false;
            character.LadderClimbingDirection = 0;
            elapsedTime = 0f;
        }

        public override void UpdateRotation(Character character, KinematicCharacterMotor motor, ref Quaternion currentRotation, float deltaTime)
        {
            switch (character.LadderClimbingStage)
            {
                case ELadderClimbingStage.Anchoring:
                    if(character.IsLadderClimbingOnTop)
                    {
                        currentRotation = Quaternion.Slerp(motor.TransientRotation, anchoringRotation, elapsedTime / anchoringDuration_ClimbingOnTop);
                    }
                    else if (character.IsLadderClimbingOnBottom)
                    {
                        currentRotation = Quaternion.Slerp(motor.TransientRotation, anchoringRotation, elapsedTime / anchoringDuration_ClimbingOnBottom);
                    }

                    break;
                case ELadderClimbingStage.DeAnchoring:
                    if (character.IsLadderClimbingOffTop)
                    {
                        currentRotation = Quaternion.Slerp(motor.TransientRotation, anchoringRotation, elapsedTime / deanchoringDuration_ClimbingOffTop);
                    }
                    else if (character.IsLadderClimbingOffBottom)
                    {
                        currentRotation = Quaternion.Slerp(motor.TransientRotation, anchoringRotation, elapsedTime / deanchoringDuration_ClimbingOffBottom);
                    }

                    break;
                case ELadderClimbingStage.Climbing:
                    currentRotation = character.CurrentClimbingLadder.transform.rotation;
                    character.IsLadderClimbing = true;

                    break;

            }
        }

        public override void UpdateState(Character character, CharacterStateMachine stateMachine)
        {
            if (character.LadderClimbingStage == ELadderClimbingStage.Anchoring)
            {
                float anchoringDuration = character.IsLadderClimbingOnTop ? anchoringDuration_ClimbingOnTop : anchoringDuration_ClimbingOnBottom;
                if (elapsedTime >= anchoringDuration)
                {
                    character.LadderClimbingStage = ELadderClimbingStage.Climbing;
                    character.IsLadderClimbingOnTop = false;
                    character.IsLadderClimbingOnBottom = false;
                }
            }
            else if (character.LadderClimbingStage == ELadderClimbingStage.Climbing)
            {
                // Detect getting off ladder during climbing
                character.CurrentClimbingLadder.ClosestPointOnLadderSegment(character.Controller.Motor.TransientPosition, out float ladderSegement);
                if (Mathf.Abs(ladderSegement) > 0.05f)
                {
                    character.LadderClimbingStage = ELadderClimbingStage.DeAnchoring;
                    elapsedTime = 0f;

                    // If we're higher than the ladder top point
                    if (ladderSegement > 0)
                    {
                        anchoringPosition = character.CurrentClimbingLadder.TopReleasePoint.position;
                        anchoringRotation = character.CurrentClimbingLadder.TopReleasePoint.rotation;
                        character.IsLadderClimbingOffTop = true;
                    }
                    // If we're lower than the ladder bottom point
                    else if (ladderSegement < 0)
                    {
                        anchoringPosition = character.CurrentClimbingLadder.BottomReleasePoint.position;
                        anchoringRotation = character.CurrentClimbingLadder.BottomReleasePoint.rotation;
                        character.IsLadderClimbingOffBottom = true;
                    }
                }
            }
            else if (character.LadderClimbingStage == ELadderClimbingStage.DeAnchoring)
            {
                float deanchoringDuration = character.IsLadderClimbingOffTop ? deanchoringDuration_ClimbingOffTop : deanchoringDuration_ClimbingOffBottom;
                if (elapsedTime >= deanchoringDuration)
                {
                    stateMachine.TransitionToState(ECharacterState.Idle);
                    character.IsLadderClimbingOffTop = false;
                    character.IsLadderClimbingOffBottom = false;
                }
            }
            elapsedTime += Time.deltaTime;
        }

        public override void UpdateVelocity(Character character, KinematicCharacterMotor motor, ref Vector3 currentVelocity, float deltaTime)
        {
            currentVelocity = Vector3.zero;

            switch (character.LadderClimbingStage)
            {
                case ELadderClimbingStage.Climbing:
                    float forwardInput = character.Controller.MovementInputVector.z;
                    currentVelocity = forwardInput * character.CurrentClimbingLadder.transform.up.normalized * character.MovementSettings.LadderClimbingSpeed;

                    character.LadderClimbingDirection = forwardInput;
                    break;

                case ELadderClimbingStage.Anchoring:
                    float anchoringDuration = character.IsLadderClimbingOnTop ? anchoringDuration_ClimbingOnTop : anchoringDuration_ClimbingOnBottom;
                    currentVelocity = motor.GetVelocityForMovePosition(
                        motor.TransientPosition,
                        Vector3.Lerp(motor.TransientPosition, anchoringPosition, elapsedTime / anchoringDuration),
                        deltaTime
                    );

                    break;
                case ELadderClimbingStage.DeAnchoring:
                    float deanchoringDuration = character.IsLadderClimbingOffTop ? deanchoringDuration_ClimbingOffTop : deanchoringDuration_ClimbingOffBottom;
                    currentVelocity = motor.GetVelocityForMovePosition(
                        motor.TransientPosition,
                        Vector3.Lerp(motor.TransientPosition, anchoringPosition, elapsedTime / deanchoringDuration),
                        deltaTime
                    );

                    break;
            }
        }
    }
}