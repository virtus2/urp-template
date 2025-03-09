using Core.Character;
using System.Collections;
using Unity.Behavior;
using UnityEngine;

namespace Core.AI
{
    /// <summary>
    /// A character controller that controlled by AI.
    /// </summary>
    public class AICharacterController : BaseCharacterController
    {
        [NaughtyAttributes.HorizontalLine]
        [Header("AI Character Controller")]
        public GameObject Target;
        public Blackboard Blackboard;

        private Coroutine attackCoroutine = null;
        private BehaviorGraphAgent agent;

        public void SetTarget(GameObject target)
        {
            Target = target;
            if (Target == null)
            {
                OrientationMethod = OrientationMethod.TowardsMovement;
            }
            else
            {
                OrientationMethod = OrientationMethod.TowardsTarget;
            }
        }

        public void Attack()
        {
            if(attackCoroutine != null)
                StopCoroutine(attackCoroutine);
            attackCoroutine = StartCoroutine(AttackInternal());
        }

        private IEnumerator AttackInternal()
        {
            AttackPressed = true;
            yield return new WaitUntil(()=> character.IsAttacking == false);
            AttackPressed = false;
        }
    }
}