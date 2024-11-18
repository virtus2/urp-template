using KinematicCharacterController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;
using UnityEngine.Windows;

namespace Core
{
    [Serializable]
    public class ComponentReferenceCharacter : ComponentReference<Character>
    {
        public ComponentReferenceCharacter(string guid) : base(guid) { }
    }

    public class Character : MonoBehaviour
    {
        public enum ECharacterControllerType { None, Player, AI };
        public enum ECharacterType { None, Player, AI };

        public ECharacterType CharacterType;

        [Header("캐릭터 설정값 ScriptableObjects")]
        public RollingSettings RollingSettings;

        public CharacterMovementComponent MovementComponent { get; private set; }
        public bool IsMoving => stateMachine.CurrentState == CharacterState.GroundMove;

        [Header("캐릭터 상태")]
        // public ICharacterController Controller;
        public BaseCharacterController Controller;
        public ECharacterControllerType ControllerType;
        public bool IsRolling = false;
        public float RollingCooldownTime;
        public bool IsAttacking = false;
        public bool IsInvincible = false;
        public bool IsDead = false;
        public bool InputEnabled = true;

        private CharacterStateMachine stateMachine;
        // TODO: AI 관련. 나중에 따로 클래스 만들수도
        public bool IsReachedDestination = false;
        public Character ChaseTarget;
        public NavMeshPath NavMeshPath;

        // 캐릭터 발걸음 이벤트
        public AudioSource FootstepAudioSource;
        public AudioClip FootstepAudioClip;
        public ParticleSystem FootstepParticleSystem;
        public void TransitionToState(CharacterState characterState)
        {
            stateMachine.TransitionToState(characterState);
        }

        private void Awake()
        {
            Controller = GetComponent<BaseCharacterController>();
            stateMachine = GetComponent<CharacterStateMachine>();
            MovementComponent = GetComponent<CharacterMovementComponent>();

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
            Character hitCharacter = hit.gameObject.GetComponent<Character>();
            if (hitCharacter)
            {
                Debug.Log($"{name} is hit {hitCharacter.name}");
                if (Controller.MovementInput.magnitude > 0.0f)
                {
                    hitCharacter.MovementComponent.AddImpulse(Controller.MovementVector);
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
    }
}
