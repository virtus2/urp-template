using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "SO_StateTransition_MovementInput", menuName = "Scriptable Objects/Character/State Transition/Movement Input")]
    public class StateTransitionSO_MovementInput : StateTransitionSO
    {
        [Header("If this value is true, State will transition when player inputs movement.")]
        public bool TransitionWhenHasMovementInput = false;

        public override bool CheckTransition(Character character, CharacterStateMachine stateMachine)
        {
            // Returns true when the player inputs movement.
            if (TransitionWhenHasMovementInput)
            {
                if (character.Controller.MovementInputVector.sqrMagnitude > float.Epsilon)
                {
                    return true;
                }
            }
            // Returns true when the player does not input movement.
            else
            {
                if (character.Controller.MovementInputVector.sqrMagnitude <= float.Epsilon)
                {
                    return true;
                }
            }
            return false;
        }
    }
}