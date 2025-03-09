using UnityEngine;

namespace Core.Character.State
{
    [CreateAssetMenu(fileName = "SO_StateTransition_Boolean", menuName = "Scriptable Objects/Character/State Transition/Boolean")]
    public class StateTransitionSO_Boolean: StateTransitionSO
    {
        [Header("If this value is true, a forced state transition will occur.")]
        public bool transition;

        public override bool CheckTransition(BaseCharacter character, CharacterStateMachine stateMachine)
        {
            return transition;
        }
    }
}