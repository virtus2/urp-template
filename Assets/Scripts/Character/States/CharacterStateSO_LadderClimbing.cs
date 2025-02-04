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
        private ELadderClimbingStage climbingStage;
        private Vector3 anchoringPosition;
        private Quaternion anchoringRotation;


        private float elapsedTime = 0f;
        private float anchoringDuration = 0.5f;
        private float climbingSpeed = 1f;

        public override void OnStateEnter(Character character, ECharacterState prevState)
        {
            character.Controller.Motor.SetMovementCollisionsSolvingActivation(false);
            character.Controller.Motor.SetGroundSolvingActivation(false);
            climbingStage = ELadderClimbingStage.Anchoring;

            // Store the target position and rotation to snap to
            anchoringPosition = character.CurrentClimbingLadder.ClosestPointOnLadderSegment(
                character.Controller.Motor.TransientPosition, out float onSegmentState);
            anchoringRotation = character.CurrentClimbingLadder.transform.rotation;
        }

        public override void OnStateExit(Character character, ECharacterState newState)
        {
            character.Controller.Motor.SetMovementCollisionsSolvingActivation(true);
            character.Controller.Motor.SetGroundSolvingActivation(true);
            climbingStage = ELadderClimbingStage.None;
        }

        public override void UpdateRotation(Character character, KinematicCharacterMotor motor, ref Quaternion currentRotation, float deltaTime)
        {
        }

        public override void UpdateState(Character character, CharacterStateMachine stateMachine)
        {
            if (climbingStage == ELadderClimbingStage.Anchoring)
            {
                if (elapsedTime >= anchoringDuration)
                {
                    climbingStage = ELadderClimbingStage.Climbing;
                }
            }
            elapsedTime += Time.deltaTime;
        }

        public override void UpdateVelocity(Character character, KinematicCharacterMotor motor, ref Vector3 currentVelocity, float deltaTime)
        {
            currentVelocity = Vector3.zero;

            switch (climbingStage)
            {
                case ELadderClimbingStage.Climbing:
                    float forwardInput = character.Controller.MovementInputVector.z;
                    currentVelocity = forwardInput * character.CurrentClimbingLadder.transform.up.normalized * climbingSpeed;
                    break;
                case ELadderClimbingStage.Anchoring:
                case ELadderClimbingStage.DeAnchoring:
                    currentVelocity = character.Controller.Motor.GetVelocityForMovePosition(
                        character.Controller.Motor.TransientPosition,
                        Vector3.Lerp(character.Controller.Motor.TransientPosition, anchoringPosition, elapsedTime / anchoringDuration),
                        deltaTime
                    );
                    break;
            }
        }
    }
}