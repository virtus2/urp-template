using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [System.Serializable]
    public class StateTransitionDefinition
    {
        public List<CharacterState> From;
        public CharacterState To;

        [SerializeReference]
        public StateTransitionSO StateTransitionRef;
        
        // TODO: 인게임에선 안쓰는 에디터용 설명 텍스트인데... 굳이 필요없으면 삭제
        [NaughtyAttributes.ResizableTextArea]
        public string Description;
    }

    [System.Serializable]
    public class CharacterStateDefinition
    {
        public CharacterState State;

        [SerializeReference]
        public CharacterStateSO CharacterStateRef;
    }

    public class CharacterStateMachine : MonoBehaviour
    {
        [NaughtyAttributes.ReadOnly]
        public CharacterState CurrentState;

        [NaughtyAttributes.ReadOnly]
        public CharacterState PreviousState;

        [Header("References for character states and transitions.")]
        [SerializeField]
        private List<StateTransitionDefinition> StateTransitions;

        [SerializeField]
        private List<CharacterStateDefinition> CharacterStates;

        private Dictionary<CharacterState, CharacterStateDefinition> runtimeCharacterStates = new Dictionary<CharacterState, CharacterStateDefinition>();
        private Dictionary<CharacterState, List<StateTransitionDefinition>> runtimeStateTransitions = new Dictionary<CharacterState, List<StateTransitionDefinition>>();
        private Character character;

        private void Awake()
        {
            InitializeRuntimeCache();

            character = GetComponent<Character>();
            if (CurrentState == CharacterState.Uninitialized)
            {
                TransitionToState(CharacterState.Idle);
            }
        }

        private void InitializeRuntimeCache()
        {
            foreach (CharacterStateDefinition stateDefinition in CharacterStates)
            {
                runtimeCharacterStates[stateDefinition.State] = stateDefinition;
            }


            foreach (StateTransitionDefinition transitionDefinition in StateTransitions)
            {
                foreach (CharacterState fromState in transitionDefinition.From)
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

        private void Update()
        {
            if (!character) return;
            CheckTransition();
            runtimeCharacterStates[CurrentState].CharacterStateRef.UpdateState(character, this);
        }

        private void CheckTransition()
        {
            if(runtimeCharacterStates.ContainsKey(CurrentState))
            {
                foreach (StateTransitionDefinition transitionDefinition in runtimeStateTransitions[CurrentState])
                {
                    if (transitionDefinition.StateTransitionRef.CheckTransition(character, this))
                    {
                        TransitionToState(transitionDefinition.To);
                    }
                }
            }
        }

        public void TransitionToState(CharacterState newState)
        {
            PreviousState = CurrentState;
            CurrentState = newState;

            OnStateExit(PreviousState, CurrentState);
            OnStateEnter(PreviousState, CurrentState);
        }

        public void OnStateEnter(CharacterState prevState, CharacterState state)
        {
            runtimeCharacterStates[state].CharacterStateRef.OnStateEnter(character, prevState);
        }

        public void OnStateExit(CharacterState state, CharacterState newState)
        {
            runtimeCharacterStates[state].CharacterStateRef.OnStateExit(character, newState);
        }

        public CharacterStateSO GetCurrentState()
        {
            return runtimeCharacterStates[CurrentState].CharacterStateRef;
        }
    }
}