using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "SO_StateTransition_AttackInput", menuName = "Scriptable Objects/Character/State Transition/Attack Input")]
    public class StateTransitionSO_AttackInput : StateTransitionSO
    {
        public override bool CheckTransition(Character character, CharacterStateMachine stateMachine)
        {
            // TODO: ���� ó���ϴ� Ŭ���� ����ų� �ƿ� StateŬ�������� ó���ϰų�...
            if (character.IsAttacking && character.AttackStage != EAttackStage.Recovery) return false;
            if (character.AttackStage == EAttackStage.Startup) return false;
            if (character.AttackStage == EAttackStage.Active) return false;

            return character.Controller.AttackPressed;
        }
    }
}