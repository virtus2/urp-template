using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Character.State
{
    [System.Serializable]
    public class StateTransitionDefinition
    {
        public List<ECharacterState> From;
        public ECharacterState To;

        [SerializeReference]
        public StateTransitionSO StateTransitionRef;
        
        // TODO: 인게임에선 안쓰는 에디터용 설명 텍스트인데... 굳이 필요없으면 삭제
        [ResizableTextArea]
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
        [ReadOnly]
        public ECharacterState CurrentState;

        [ReadOnly]
        public ECharacterState PreviousState;

        [Header("References for character states and transitions.")]
        [SerializeField]
        private List<StateTransitionDefinition> StateTransitions;

        [SerializeField]
        private List<CharacterStateDefinition> CharacterStates;

        private Dictionary<ECharacterState, CharacterState> runtimeCharacterStates = new Dictionary<ECharacterState, CharacterState>();
        private Dictionary<ECharacterState, List<StateTransitionDefinition>> runtimeStateTransitions = new Dictionary<ECharacterState, List<StateTransitionDefinition>>();
        private BaseCharacter character;

        private void Awake()
        {
            InitializeRuntimeCache();

            character = GetComponent<BaseCharacter>();
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
        [Button]
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