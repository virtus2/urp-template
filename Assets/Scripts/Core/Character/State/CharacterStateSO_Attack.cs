using KinematicCharacterController;
using UnityEngine;

namespace Core.Character.State
{
    [System.Serializable]
    public class CharacterStateData_Attack
    {
        public float AttackVelocityMultiplier = 1f;
        public float AttackVelocity = 1f;
    }

    [CreateAssetMenu(fileName = "SO_CharacterState_Attack", menuName = "Scriptable Objects/Character/State/Attack")]
    public class CharacterStateSO_Attack : CharacterStateSO
    {
        [SerializeField]
        private CharacterStateData_Attack data;
        public override CharacterState CreateInstance()
        {
            CharacterState_Attack state = new CharacterState_Attack(data);
            return state;
        }
    }

    public class CharacterState_Attack : CharacterState
    {
        private float elapsedTime = 0.0f;
        private CharacterStateData_Attack data;

        public CharacterState_Attack(CharacterStateData_Attack data)
        {
            this.data = data;
        }

        public override void OnStateEnter(BaseCharacter character, ECharacterState prevState)
        {
            elapsedTime = 0.0f;

            character.IsAttacking = true;
            character.AttackComboCount = 1;
        }

        public override void OnStateExit(BaseCharacter character, ECharacterState newState)
        {
            if (CharacterUtility.IsAttackState(newState) == false)
            {
                character.IsAttacking = false;
                character.AttackStage = EAttackStage.None;
                character.AttackComboCount = 0;
            }
        }

        public override void UpdateState(BaseCharacter character, CharacterStateMachine stateMachine)
        {
            elapsedTime += Time.deltaTime;

            // TODO: StateTransitionSO 만들어서 해결? 아니면 그냥 StateSO에 하드코딩?
            if (character.IsAttacking == false)
            {
                stateMachine.TransitionToState(ECharacterState.Idle);
            }
        }

        public override void UpdateVelocity(BaseCharacter character, KinematicCharacterMotor motor, ref Vector3 currentVelocity, float deltaTime)
        {
            /*
            currentVelocity = motor.CharacterForward *
                data.AttackVelocity *
                data.AttackVelocityMultiplier;
            */
        }

        public override void UpdateRotation(BaseCharacter character, KinematicCharacterMotor motor, ref Quaternion currentRotation, float deltaTime)
        {
         
        }
    }
}