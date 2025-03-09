using Core.Character.State;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Core.Character
{
    [Serializable]
    public class ComponentReferenceCharacter : ComponentReference<BaseCharacter>
    {
        public ComponentReferenceCharacter(string guid) : base(guid) { }
    }

    public class BaseCharacter : MonoBehaviour
    {
        public enum ECharacterControllerType { None, Player, AI };
        public enum ECharacterType { None, Player, AI };

        public bool IsMoving => stateMachine.CurrentState == ECharacterState.GroundMove;

        public ECharacterType CharacterType;

        [Header("Character Settings")]
        public RollingSettings RollingSettings;
        public MovementSettings MovementSettings;

        [Header("Character Controlls")]
        public BaseCharacterController Controller;
        public ECharacterControllerType ControllerType;
        public bool InputEnabled = true;

        [Header("Character States")]
        public bool IsInvincible = false;
        public bool IsDead = false;

        [Range(0f, 2f)]
        public float MotionSpeed = 1.0f;
        public float TimeScale = 1.0f; // TODO: 아~주 나중에 캐릭터들마다 개별적으로 타임 스케일 설정 기능이 생겨야 한다면 구현.

        [Header("Rolling")]
        public bool IsRolling = false;
        public float RollingCooldownTime;

        [Header("Attack")]
        [Tooltip("Is character attacking?")]
        public bool IsAttacking = false;

        [Header("Ladder Climbing")]
        public Level.Placeable.Ladder CurrentClimbingLadder;
        public float LadderClimbingDirection = 0;
        public bool IsLadderClimbing = false;
        public bool IsLadderClimbingOnTop = false;
        public bool IsLadderClimbingOnBottom = false;
        public bool IsLadderClimbingOffTop = false;
        public bool IsLadderClimbingOffBottom = false;
        public ELadderClimbingStage LadderClimbingStage;

        [Tooltip("Attack stage of the character.")]
        public EAttackStage AttackStage = EAttackStage.None;

        // TODO: Tooltips , 네이밍 별로인거같은데 바꾸기
        // 공격 콤보 단계. 
        public int AttackComboCount = 0;

        public Action<BaseCharacter, CharacterStateMachine> OnAttackFinishedCallback;

        

        // TODO: AI 관련. 나중에 따로 클래스 만들수도
        public bool IsReachedDestination = false;
        public BaseCharacter ChaseTarget;
        public NavMeshPath NavMeshPath;

        // 캐릭터 발걸음 이벤트
        public AudioSource FootstepAudioSource;
        public AudioClip FootstepAudioClip;
        public ParticleSystem FootstepParticleSystem;

        private CharacterStateMachine stateMachine;
        private SkinnedMeshRenderer skinnedMeshRenderer;


        private void Awake()
        {
            Controller = GetComponent<BaseCharacterController>();
            stateMachine = GetComponent<CharacterStateMachine>();
            skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            NavMeshPath = new NavMeshPath();

            Initialize();
        }

        private void Initialize()
        {
            IsRolling = false;
            RollingCooldownTime = RollingSettings.RollingCooldownTime;
        }

        private void Update()
        {

            // 게임 시스템
            UpdateRollCooldownTime();
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            BaseCharacter hitCharacter = hit.gameObject.GetComponent<BaseCharacter>();
            if (hitCharacter)
            {
                Debug.Log($"{name} is hit {hitCharacter.name}");
                if (Controller.MovementInputVector.magnitude > 0.0f)
                {
                    // hitCharacter.MovementComponent.AddImpulse(Controller.MovementVector);
                    // hitCharacter.AddForce(Controller.MovementVector);
                }
            }
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                // TODO: 발소리 내기?
                if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo))
                {
                    FootstepAudioSource.clip = FootstepAudioClip;
                    FootstepAudioSource.Play();
                    FootstepParticleSystem.Emit(1);
                    Debug.Log(hitInfo.transform.gameObject.name);
                }
            }
        }

        private void OnAttackStartup(AnimationEvent animationEvent)
        {
            Debug.Log("OnAttackStartup");
            AttackStage = EAttackStage.Startup;
            skinnedMeshRenderer.material.color = Color.green;
        }

        private void OnAttackActive(AnimationEvent animationEvent)
        {
            Debug.Log("OnAttackActive");
            AttackStage = EAttackStage.Active;
            skinnedMeshRenderer.material.color = Color.red;
        }

        private void OnAttackRecovery(AnimationEvent animationEvent)
        {
            Debug.Log("OnAttackRecovery");
            AttackStage = EAttackStage.Recovery;
            skinnedMeshRenderer.material.color = Color.blue;
        }

        private void OnAttackFinished(AnimationEvent animationEvent)
        {
            Debug.Log("OnAttackFinished");
            skinnedMeshRenderer.material.color = Color.white;
            // TODO: 여기서 직접 IsAttacking = false해주는게 좋을지
            AttackStage = EAttackStage.None;
            IsAttacking = false;
            AttackComboCount = 0;
            // 아니면 State에서 콜백 등록해서 호출해주는게 나을지
            // 모르겠다~
            OnAttackFinishedCallback?.Invoke(this, stateMachine);
        }

        private void UpdateRollCooldownTime()
        {
            // 구르기중이 아닐때에만 구르기 쿨다운시간이 흐른다.
            // TODO: 쿨다운타임 있는 행동들은 Ability클래스를 따로 만들어서 하는게... 나을려나... 굳이인가?
            if (!IsRolling)
            {
                RollingCooldownTime += Time.deltaTime;
            }
        }

        /// <summary>
        /// 현재 캐릭터가 구르기 행동을 할 수 있는지 검사한다.
        /// </summary>
        public bool CanRoll()
        {
            bool IsOnCooldown = RollingCooldownTime <= RollingSettings.RollingCooldownTime;
            // 캐릭터의 현재 상태가 구르기 상태로 넘어갈 수 있는지도 체크해야할 수도 있음.
            // ...
            return !IsOnCooldown && !IsRolling;
        }


        private void OnDrawGizmos()
        {
            // Velocity
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Controller.Motor.TransientPosition, Controller.Motor.TransientPosition + Controller.Motor.Velocity);

            // Rotation
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Controller.Motor.TransientPosition, Controller.Motor.TransientPosition + Controller.Motor.CharacterForward);
        }
    }
}
