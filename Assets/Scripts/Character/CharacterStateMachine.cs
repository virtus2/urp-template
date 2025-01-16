using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [System.Serializable]
    public class StateTransitionDefinition
    {
        public List<ECharacterState> From;
        public ECharacterState To;

        [SerializeReference]
        public StateTransitionSO StateTransitionRef;
        
        // TODO: �ΰ��ӿ��� �Ⱦ��� �����Ϳ� ���� �ؽ�Ʈ�ε�... ���� �ʿ������ ����
        [NaughtyAttributes.ResizableTextArea]
        public string Description;
    }

    [System.Serializable]
    public class CharacterStateDefinition
    {
        public ECharacterState State;

        [SerializeReference]
        public CharacterStateSO CharacterStateRef;
    }

    public class CharacterStateMachine : MonoBehaviour
    {
        [NaughtyAttributes.ReadOnly]
        public ECharacterState CurrentState;

        [NaughtyAttributes.ReadOnly]
        public ECharacterState PreviousState;

        [Header("References for character states and transitions.")]
        [SerializeField]
        private List<StateTransitionDefinition> StateTransitions;

        [SerializeField]
        private List<CharacterStateDefinition> CharacterStates;

        private Dictionary<ECharacterState, CharacterState> runtimeCharacterStates = new Dictionary<ECharacterState, CharacterState>();
        private Dictionary<ECharacterState, List<StateTransitionDefinition>> runtimeStateTransitions = new Dictionary<ECharacterState, List<StateTransitionDefinition>>();
        private Character character;

        private void Awake()
        {
            InitializeRuntimeCache();

            character = GetComponent<Character>();
        }

        private void InitializeRuntimeCache()
        {
            foreach (CharacterStateDefinition stateDefinition in CharacterStates)
            {
                runtimeCharacterStates[stateDefinition.State] = stateDefinition.CharacterStateRef.CreateInstance();
            }

            foreach (StateTransitionDefinition transitionDefinition in StateTransitions)
            {
                foreach (ECharacterState fromState in transitionDefinition.From)
                {
                    if (runtimeStateTransitions.ContainsKey(fromState))
                    {
                        runtimeStateTransitions[fromState].Add(transitionDefinition);
                    }
                    else
                    {
                        runtimeStateTransitions.Add(fromState, new List<StateTransitionDefinition>() { transitionDefinition });
                    }
                }
            }
        }

#if UNITY_EDITOR
        [NaughtyAttributes.Button]
        private void Recache()
        {
            runtimeCharacterStates.Clear();
            runtimeStateTransitions.Clear();
            InitializeRuntimeCache();
        }
#endif 

        private void Update()
        {
            if (!character) return;
            CheckTransition();
            runtimeCharacterStates[CurrentState].UpdateState(character, this);
        }

        private void CheckTransition()
        {
            if(runtimeStateTransitions.ContainsKey(CurrentState))
            {
                foreach (StateTransitionDefinition transitionDefinition in runtimeStateTransitions[CurrentState])
                {
                    if (transitionDefinition.StateTransitionRef.CheckTransition(character, this))
                    {
                        TransitionToState(transitionDefinition.To);
                        return;
                    }
                }
            }
        }

        public void TransitionToState(ECharacterState newState)
        {
            PreviousState = CurrentState;
            CurrentState = newState;

            OnStateExit(PreviousState, CurrentState);
            OnStateEnter(PreviousState, CurrentState);
        }

        public void OnStateEnter(ECharacterState prevState, ECharacterState state)
        {
            runtimeCharacterStates[state].OnStateEnter(character, prevState);
        }

        public void OnStateExit(ECharacterState state, ECharacterState newState)
        {
            runtimeCharacterStates[state].OnStateExit(character, newState);
        }

        public CharacterState GetCurrentState()
        {
            return runtimeCharacterStates[CurrentState];
        }
    }
}