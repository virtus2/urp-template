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

        public override void OnStateEnter(Character character, ECharacterState prevState)
        {
            character.Controller.Motor.SetMovementCollisionsSolvingActivation(false);
            character.Controller.Motor.SetGroundSolvingActivation(false);
            climbingStage = ELadderClimbingStage.Anchoring;

            // Store the target position and rotation to snap to
            _ladderTargetPosition = _activeLadder.ClosestPointOnLadderSegment(Motor.TransientPosition, out _onLadderSegmentState);
            _ladderTargetRotation = _activeLadder.transform.rotation;
            break;
        }

        public override void OnStateExit(Character character, ECharacterState newState)
        {
        }

        public override void UpdateRotation(Character character, KinematicCharacterMotor motor, ref Quaternion currentRotation, float deltaTime)
        {
        }

        public override void UpdateState(Character character, CharacterStateMachine stateMachine)
        {
            
        }

        public override void UpdateVelocity(Character character, KinematicCharacterMotor motor, ref Vector3 currentVelocity, float deltaTime)
        {
        }
    }
}