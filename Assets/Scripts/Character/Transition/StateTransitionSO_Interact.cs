using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "SO_StateTransition_Interact", menuName = "Scriptable Objects/Character/State Transition/Interact")]
    public class StateTransitionSO_Interact: StateTransitionSO
    {
        // TODO: ��ٸ� Ÿ�� ��ȣ�ۿ�� �ٸ� ��ȣ�ۿ� ���� �и�
        public LayerMask InteractionLayer;

        [NaughtyAttributes.Tag]
        public string Tag;

        public override bool CheckTransition(Character character, CharacterStateMachine stateMachine)
        {
            if (character.Controller.InteractPressed)
            {
                if (character.Controller.CharacterOverlap(InteractionLayer, QueryTriggerInteraction.UseGlobal, out Collider[] overlappedColliders) > 0)
                {
                    Debug.Log(overlappedColliders[0].name);
                    if (overlappedColliders[0].CompareTag(Tag))
                    {
                        Ladder ladder = overlappedColliders[0].GetComponent<Ladder>();
                        if (ladder != null) 
                        {
                            character.CurrentClimbingLadder = ladder;
                        }
                        return true;
                    }
                }
            }
            return false;
        }
    }
}