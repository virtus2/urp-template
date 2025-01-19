using KinematicCharacterController;
using UnityEngine;

namespace Core
{
    // TODO: CharacterStateSO_Attack으로만 사용하고 콤보는 데이터로 처리
    // 근데 어택상태마다 다른동작해야되면 그냥 클래스 따로 만드는게 나을수도있음.
    // ex: int attackComboCount = 0;
    [CreateAssetMenu(fileName = "SO_CharacterState_Attack2", menuName = "Scriptable Objects/Character/State/Attack2")]
    public class CharacterStateSO_Attack2 : CharacterStateSO
    {
        [SerializeField]
        private CharacterStateData_Attack data;
        public override CharacterState CreateInstance()
        {
            // TODO: 실제 로직 클래스 네이밍 실수 방지법 필요
            CharacterState_Attack2 state = new CharacterState_Attack2(data);
            return state;
        }
    }

    public class CharacterState_Attack2 : CharacterState
    {
        private float elapsedTime = 0.0f;
        private CharacterStateData_Attack data;

        public CharacterState_Attack2(CharacterStateData_Attack data)
        {
            this.data = data;
        }

        public override void OnStateEnter(Character character, ECharacterState prevState)
        {
            elapsedTime = 0.0f;

            character.IsAttacking = true;
            character.AttackComboCount = 2;
        }

        public override void OnStateExit(Character character, ECharacterState newState)
        {
            if (CharacterUtility.IsAttackState(newState) == false)
            {
                character.IsAttacking = false;
                character.AttackStage = EAttackStage.None;
                character.AttackComboCount = 0;
            }
        }

        public override void UpdateState(Character character, CharacterStateMachine stateMachine)
        {
            elapsedTime += Time.deltaTime;

            // TODO: StateTransitionSO 만들어서 해결? 아니면 그냥 StateSO에 하드코딩?
            if (character.IsAttacking == false)
            {
                stateMachine.TransitionToState(ECharacterState.Idle);
            }
        }

        public override void UpdateVelocity(Character character, KinematicCharacterMotor motor, ref Vector3 currentVelocity, float deltaTime)
        {
            currentVelocity = motor.CharacterForward *
                data.AttackVelocity *
                data.AttackVelocityMultiplier;
        }

        public override void UpdateRotation(Character character, KinematicCharacterMotor motor, ref Quaternion currentRotation, float deltaTime)
        {
         
        }
    }
}