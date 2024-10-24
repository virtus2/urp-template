using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine.Animations.Rigging;
using UnityEditor.ShaderGraph.Internal;

namespace Core
{
    [RequireComponent(typeof(Character), typeof(Animator))]
    public class CharacterAnimationComponent : MonoBehaviour
    {
        [SerializeField] private AnimatorController AnimatorController;

        [Header("애니메이터 States")]
        [SerializeField] private string Animator_State_Name_Locomotion = "Locomotion";
        [SerializeField] private string Animator_State_Name_Walk = "Walk";
        [SerializeField] private string Animator_State_Name_Run = "Run";

        [Header("애니메이터 Parameters")]
        [SerializeField] private string Animator_Parameter_Name_Speed = "Speed";
        [SerializeField] private string Animator_Parameter_Name_IsGrounded = "IsGrounded";
        [SerializeField] private string Animator_Parameter_Name_IsRolling = "IsRolling";
        [SerializeField] private string Animator_Parameter_Name_IsDead = "IsDead";
        [SerializeField] private string Animator_Parameter_Name_MotionSpeed = "MotionSpeed";
        [SerializeField] private string Animator_Parameter_Name_Attack = "Attack";
        [SerializeField] private string Animator_Parameter_Name_FreeFall = "FreeFall";

        [Header("애니메이터 블렌드 트리 Thresholds")]
        [SerializeField] private float Animator_Threshold_Walk = 0.5f;
        [SerializeField] private float Animator_Threshold_Run = 1f;

        [Header("애니메이터 Layers")]
        [SerializeField] private string Animator_Layer_Name_BaseLayer = "Base Layer";
        [SerializeField] private string Animator_Layer_Name_UpperLayer = "Upper Layer";

        private int Animator_Parameter_Hash_Speed;
        private int Animator_Parameter_Hash_IsGrounded;
        private int Animator_Parameter_Hash_IsRolling;
        private int Animator_Parameter_Hash_IsDead;
        private int Animator_Parameter_Hash_MotionSpeed;
        private int Animator_Parameter_Hash_Attack;
        private int Animator_Parameter_Hash_FreeFall;

        private int Animator_Layer_Index_BaseLayer;
        private int Animator_Layer_Index_UpperLayer;

        private Character character;
        private Animator animator;
        private CharacterMovementComponent movementComponent;
        private RuntimeAnimatorController runtimeAnimatorController;
        
        private MultiAimConstraint multiAimConstraint;

        private void Awake()
        {
            character = GetComponent<Character>();
            animator = GetComponent<Animator>();
            movementComponent = GetComponent<CharacterMovementComponent>();

            runtimeAnimatorController = animator.runtimeAnimatorController;

            Animator_Parameter_Hash_Speed = Animator.StringToHash(Animator_Parameter_Name_Speed);
            Animator_Parameter_Hash_IsGrounded = Animator.StringToHash(Animator_Parameter_Name_IsGrounded);
            Animator_Parameter_Hash_IsRolling = Animator.StringToHash(Animator_Parameter_Name_IsRolling);
            Animator_Parameter_Hash_IsDead = Animator.StringToHash(Animator_Parameter_Name_IsDead);
            Animator_Parameter_Hash_MotionSpeed = Animator.StringToHash(Animator_Parameter_Name_MotionSpeed);
            Animator_Parameter_Hash_Attack = Animator.StringToHash(Animator_Parameter_Name_Attack);
            Animator_Parameter_Hash_FreeFall = Animator.StringToHash(Animator_Parameter_Name_FreeFall);

            Animator_Layer_Index_BaseLayer = animator.GetLayerIndex(Animator_Layer_Name_BaseLayer);
            Animator_Layer_Index_UpperLayer = animator.GetLayerIndex(Animator_Layer_Name_UpperLayer);
        }

        private void Update()
        {
            UpdateLocomotionParameters();

            animator.SetBool(Animator_Parameter_Hash_Attack, character.IsAttacking); // TODO: 애니메이터 컨트롤러에 매개변수 추가
            animator.SetBool(Animator_Parameter_Hash_IsGrounded, movementComponent.IsGrounded);
            animator.SetBool(Animator_Parameter_Hash_IsRolling, character.IsRolling);
            animator.SetBool(Animator_Parameter_Hash_IsDead, character.IsDead);
            /*
            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;
            */
        }

        private void UpdateLocomotionParameters()
        {
            // 걷기 상태일때는 Animator_Threshold_Walk값에, 뜀 상태일때는 Animator_Threshold_Run값에 맞춰준다.
            float speedRatio = 0f;
            if (movementComponent.HorizontalSpeed <= movementComponent.MovementSettings.WalkSpeed)
            {
                speedRatio = Animator_Threshold_Walk * (movementComponent.HorizontalSpeed / movementComponent.MovementSettings.WalkSpeed);
            }
            else
            {
                speedRatio = Animator_Threshold_Walk + 
                    ((movementComponent.HorizontalSpeed - movementComponent.MovementSettings.WalkSpeed) / (movementComponent.MovementSettings.RunSpeed - movementComponent.MovementSettings.WalkSpeed)) * 
                    (Animator_Threshold_Run - Animator_Threshold_Walk);
            }
            animator.SetFloat(Animator_Parameter_Hash_Speed, speedRatio);

            // TODO: 이속 빨라지거나 느려지면(ex: 디버프) 로코모션 재생속도 증감
            animator.SetFloat(Animator_Parameter_Hash_MotionSpeed, 1f); 
        }
    }
}