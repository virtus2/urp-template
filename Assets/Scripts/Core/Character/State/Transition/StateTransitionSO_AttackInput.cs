using UnityEngine;

namespace Core.Character.State
{
    [CreateAssetMenu(fileName = "SO_StateTransition_AttackInput", menuName = "Scriptable Objects/Character/State Transition/Attack Input")]
    public class StateTransitionSO_AttackInput : StateTransitionSO
    {
        public override bool CheckTransition(BaseCharacter character, CharacterStateMachine stateMachine)
        {
            // TODO: 따로 처리하는 클래스 만들거나 아예 State클래스에서 처리하거나...
            if (character.IsAttacking && character.AttackStage != EAttackStage.Recovery) return false;
            if (character.AttackStage == EAttackStage.Startup) return false;
            if (character.AttackStage == EAttackStage.Active) return false;

            return character.Controller.AttackPressed;
        }
    }
}