using Core.Character.State;
using UnityEngine;

namespace Core.Character
{
    public static class CharacterUtility
    {
        public static bool IsAttackState(ECharacterState state)
        {
            return state == ECharacterState.Attack || state == ECharacterState.Attack2;
        }
    }
}