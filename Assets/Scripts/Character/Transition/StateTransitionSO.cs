using UnityEngine;

namespace Core
{
    public abstract class StateTransitionSO : ScriptableObject
    {
        public abstract bool CheckTransition(Character character, CharacterStateMachine stateMachine);
    }
}