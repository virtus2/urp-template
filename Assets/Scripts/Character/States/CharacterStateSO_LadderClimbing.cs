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
        private float anchoringDuration = 0.5f;

        public override void OnStateEnter(Character character, ECharacterState prevState)
        {
            character.Controller.Motor.SetMovementCollisionsSolvingActivation(false);
            character.Controller.Motor.SetGroundSolvingActivation(false);
            character.LadderClimbingStage = ELadderClimbingStage.Anchoring;
            // character.CurrentClimbingLadder is set by StateTransitionSO_Interact
            elapsedTime = 0f;

            // Store the target position and rotation to snap to
            anchoringPosition = character.CurrentClimbingLadder.ClosestPointOnLadderSegment(
                character.Controller.Motor.TransientPosition, out float onSegmentState);
            anchoringRotation = character.CurrentClimbingLadder.transform.rotation;
        }

        public override void OnStateExit(Character character, ECharacterState newState)
        {
            character.Controller.Motor.SetMovementCollisionsSolvingActivation(true);
            character.Controller.Motor.SetGroundSolvingActivation(true);
            character.LadderClimbingStage = ELadderClimbingStage.None;
            character.CurrentClimbingLadder = null;
            elapsedTime = 0f;
        }

        public override void UpdateRotation(Character character, KinematicCharacterMotor motor, ref Quaternion currentRotation, float deltaTime)
        {
            switch (character.LadderClimbingStage)
            {
                case ELadderClimbingStage.Anchoring:
                case ELadderClimbingStage.DeAnchoring:
                    currentRotation = Quaternion.Slerp(motor.TransientRotation, anchoringRotation, elapsedTime / anchoringDuration);

                    break;
                case ELadderClimbingStage.Climbing:
                    currentRotation = character.CurrentClimbingLadder.transform.rotation;

                    break;

            }
        }

        public override void UpdateState(Character character, CharacterStateMachine stateMachine)
        {
            if (character.LadderClimbingStage == ELadderClimbingStage.Anchoring)
            {
                if (elapsedTime >= anchoringDuration)
                {
                    character.LadderClimbingStage = ELadderClimbingStage.Climbing;
                }
            }
            if (character.LadderClimbingStage == ELadderClimbingStage.Climbing)
            {
                // Detect getting off ladder during climbing
                character.CurrentClimbingLadder.ClosestPointOnLadderSegment(character.Controller.Motor.TransientPosition, out float ladderSegement);
                if (Mathf.Abs(ladderSegement) > 0.05f)
                {
                    character.LadderClimbingStage = ELadderClimbingStage.DeAnchoring;

                    // TODO: 사다리 탄 후 도착지 설정
                    /*
                    // If we're higher than the ladder top point
                    if (ladderSegement > 0)
                    {
                        anchoringPosition = character.CurrentClimbingLadder.TopReleasePoint.position;
                        anchoringRotation = character.CurrentClimbingLadder.TopReleasePoint.rotation;
                    }
                    // If we're lower than the ladder bottom point
                    else if (ladderSegement < 0)
                    {
                        anchoringPosition = character.CurrentClimbingLadder.BottomReleasePoint.position;
                        anchoringRotation = character.CurrentClimbingLadder.BottomReleasePoint.rotation;
                    }
                    */
                }
            }
            if(character.LadderClimbingStage == ELadderClimbingStage.DeAnchoring)
            {
                stateMachine.TransitionToState(ECharacterState.Idle);
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

                    break;

                // TODO: 사다리 탄 후 도착지 설정
                case ELadderClimbingStage.Anchoring:
                case ELadderClimbingStage.DeAnchoring:
                    currentVelocity = motor.GetVelocityForMovePosition(
                        motor.TransientPosition,
                        Vector3.Lerp(motor.TransientPosition, anchoringPosition, elapsedTime / anchoringDuration),
                        deltaTime
                    );

                    break;
            }
        }
    }
}