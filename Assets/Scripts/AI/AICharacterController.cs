using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Core
{
    /// <summary>
    /// AI를 기반으로 한 캐릭터 컨트롤러
    /// </summary>
    public class AICharacterController : BaseCharacterController
    {
        // TODO: FSM이나 Behaviour Tree로 현재 AI가 해야 할 행동을 정의하고, 그에 맞춰 컨트롤러의 MovementInput등의 값을 설정한다.
        public GameObject chaseTarget;
        public AIStateMachine aiStateMachine;

        public Character testAICharacter;

        private void Awake()
        {
            aiStateMachine = GetComponent<AIStateMachine>();

        }
        private void Start()
        {
            SetCharacter(testAICharacter);
            testAICharacter.SetController(this);
        }
        
        public override void SetCharacter(Character character)
        {
            base.SetCharacter(character); 

            aiStateMachine = character.GetComponent<AIStateMachine>();
            if(!aiStateMachine)
            {
                Debug.LogWarning($"{character.name}에 AIStateMachine 컴포넌트가 없습니다.");
            }
        }
    }
}