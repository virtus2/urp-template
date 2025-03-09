using UnityEngine;

namespace Core.Character.State
{
    public abstract class StateTransitionSO : ScriptableObject
    {
        public abstract bool CheckTransition(BaseCharacter character, CharacterStateMachine stateMachine);
    }
}