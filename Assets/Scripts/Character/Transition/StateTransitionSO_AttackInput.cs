using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "SO_StateTransition_AttackInput", menuName = "Scriptable Objects/Character/State Transition/Attack Input")]
    public class StateTransitionSO_AttackInput : StateTransitionSO
    {
        public override bool CheckTransition(Character character, CharacterStateMachine stateMachine)
        {
            return character.Controller.AttackPressed;
        }
    }
}