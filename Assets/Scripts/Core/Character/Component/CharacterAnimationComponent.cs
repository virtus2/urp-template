using UnityEngine;
using UnityEditor.Animations;
using UnityEngine.Animations.Rigging;

namespace Core.Character.Component
{
    [RequireComponent(typeof(BaseCharacter), typeof(Animator))]
    public class CharacterAnimationComponent : MonoBehaviour
    {
        [SerializeField] private AnimatorController AnimatorController;

        [Header("Animator State Machine States")]
        [SerializeField] private string Animator_State_Name_Locomotion = "Locomotion";
        [SerializeField] private string Animator_State_Name_Walk = "Walk";
        [SerializeField] private string Animator_State_Name_Sprint = "Sprint";

        [Header("Animator Parameters")]
        [SerializeField] private string Animator_Parameter_Name_Speed = "Speed";
        [SerializeField] private string Animator_Parameter_Name_IsGrounded = "IsGrounded";
        [SerializeField] private string Animator_Parameter_Name_IsRolling = "IsRolling";
        [SerializeField] private string Animator_Parameter_Name_IsDead = "IsDead";
        [SerializeField] private string Animator_Parameter_Name_MotionSpeed = "MotionSpeed";
        [SerializeField] private string Animator_Parameter_Name_IsAttacking = "IsAttacking";
        [SerializeField] private string Animator_Parameter_Name_AttackComboCount = "AttackComboCount";
        [SerializeField] private string Animator_Parameter_Name_FreeFall = "FreeFall";
        [SerializeField] private string Animator_Parameter_Name_Forward = "Forward";
        [SerializeField] private string Animator_Parameter_Name_Right = "Right";
        [SerializeField] private string Animator_Parameter_Name_IsLadderClimbing = "IsLadderClimbing";
        [SerializeField] private string Animator_Parameter_Name_IsLadderClimbingOnTop = "IsLadderClimbingOnTop";
        [SerializeField] private string Animator_Parameter_Name_IsLadderClimbingOnBottom = "IsLadderClimbingOnBottom";
        [SerializeField] private string Animator_Parameter_Name_IsLadderClimbingOffTop = "IsLadderClimbingOffTop";
        [SerializeField] private string Animator_Parameter_Name_IsLadderClimbingOffBottom = "IsLadderClimbingOffBottom";
        [SerializeField] private string Animator_Parameter_Name_LadderClimbingDirection= "LadderClimbingDirection";
        private int Animator_Parameter_Hash_Speed;
        private int Animator_Parameter_Hash_IsGrounded;
        private int Animator_Parameter_Hash_IsRolling;
        private int Animator_Parameter_Hash_IsDead;
        private int Animator_Parameter_Hash_MotionSpeed;
        private int Animator_Parameter_Hash_IsAttacking;
        private int Animator_Parameter_Hash_AttackComboCount;
        private int Animator_Parameter_Hash_FreeFall;
        private int Animator_Parameter_Hash_Forward;
        private int Animator_Parameter_Hash_Right;
        private int Animator_Parameter_Hash_IsLadderClimbing;
        private int Animator_Parameter_Hash_IsLadderClimbingOnTop;
        private int Animator_Parameter_Hash_IsLadderClimbingOnBottom;
        private int Animator_Parameter_Hash_IsLadderClimbingOffTop;
        private int Animator_Parameter_Hash_IsLadderClimbingOffBottom;
        private int Animator_Parameter_Hash_LadderClimbingDirection;

        [Header("Animator Locomotion BlendTree Threshold")]
        [Header("Do not change this until you understand this!")]
        [SerializeField] private float Animator_Threshold_Walk = 0.5f;
        [SerializeField] private float Animator_Threshold_Sprint = 1f;

        [Header("Animator Layers")]
        [SerializeField] private string Animator_Layer_Name_BaseLayer = "Base Layer";
        [SerializeField] private string Animator_Layer_Name_UpperLayer = "Upper Layer";
        private int Animator_Layer_Index_BaseLayer;
        private int Animator_Layer_Index_UpperLayer;

        [Header("Animation Curves")]
        [SerializeField] private string Animation_Curve_Name_Velocity_Attack_Forward = "VelocityAttackForward";
        private int Animation_Curve_Hash_Velocity_Attack_Forward;



        private BaseCharacter character;
        private Animator animator;
        private RuntimeAnimatorController runtimeAnimatorController;

        private MultiAimConstraint multiAimConstraint;

        private void Awake()
        {
            character = GetComponent<BaseCharacter>();
            animator = GetComponent<Animator>();

            runtimeAnimatorController = animator.runtimeAnimatorController;

            Animator_Parameter_Hash_Speed = Animator.StringToHash(Animator_Parameter_Name_Speed);
            Animator_Parameter_Hash_IsGrounded = Animator.StringToHash(Animator_Parameter_Name_IsGrounded);
            Animator_Parameter_Hash_IsRolling = Animator.StringToHash(Animator_Parameter_Name_IsRolling);
            Animator_Parameter_Hash_IsDead = Animator.StringToHash(Animator_Parameter_Name_IsDead);
            Animator_Parameter_Hash_MotionSpeed = Animator.StringToHash(Animator_Parameter_Name_MotionSpeed);
            Animator_Parameter_Hash_IsAttacking = Animator.StringToHash(Animator_Parameter_Name_IsAttacking);
            Animator_Parameter_Hash_AttackComboCount = Animator.StringToHash(Animator_Parameter_Name_AttackComboCount);
            Animator_Parameter_Hash_FreeFall = Animator.StringToHash(Animator_Parameter_Name_FreeFall);
            Animator_Parameter_Hash_Forward = Animator.StringToHash(Animator_Parameter_Name_Forward);
            Animator_Parameter_Hash_Right = Animator.StringToHash(Animator_Parameter_Name_Right);
            Animator_Parameter_Hash_IsLadderClimbing = Animator.StringToHash(Animator_Parameter_Name_IsLadderClimbing);
            Animator_Parameter_Hash_IsLadderClimbingOnTop = Animator.StringToHash(Animator_Parameter_Name_IsLadderClimbingOnTop);
            Animator_Parameter_Hash_IsLadderClimbingOnBottom = Animator.StringToHash(Animator_Parameter_Name_IsLadderClimbingOnBottom);
            Animator_Parameter_Hash_IsLadderClimbingOffTop= Animator.StringToHash(Animator_Parameter_Name_IsLadderClimbingOffTop);
            Animator_Parameter_Hash_IsLadderClimbingOffBottom = Animator.StringToHash(Animator_Parameter_Name_IsLadderClimbingOffBottom);
            Animator_Parameter_Hash_LadderClimbingDirection = Animator.StringToHash(Animator_Parameter_Name_LadderClimbingDirection);

            Animator_Layer_Index_BaseLayer = animator.GetLayerIndex(Animator_Layer_Name_BaseLayer);
            Animator_Layer_Index_UpperLayer = animator.GetLayerIndex(Animator_Layer_Name_UpperLayer);

            Animation_Curve_Hash_Velocity_Attack_Forward = Animator.StringToHash(Animation_Curve_Name_Velocity_Attack_Forward);
        }

        private void Update()
        {
            UpdateLocomotionParameters();

            animator.SetBool(Animator_Parameter_Hash_IsAttacking, character.IsAttacking);
            animator.SetInteger(Animator_Parameter_Hash_AttackComboCount, character.AttackComboCount);
            animator.SetBool(Animator_Parameter_Hash_IsGrounded, character.Controller.IsGrounded);
            animator.SetBool(Animator_Parameter_Hash_IsRolling, character.IsRolling);
            animator.SetBool(Animator_Parameter_Hash_IsDead, character.IsDead);
            animator.SetBool(Animator_Parameter_Hash_IsLadderClimbing, character.IsLadderClimbing);
            animator.SetBool(Animator_Parameter_Hash_IsLadderClimbingOnTop, character.IsLadderClimbingOnTop);
            animator.SetBool(Animator_Parameter_Hash_IsLadderClimbingOnBottom, character.IsLadderClimbingOnBottom);
            animator.SetBool(Animator_Parameter_Hash_IsLadderClimbingOffTop, character.IsLadderClimbingOffTop);
            animator.SetBool(Animator_Parameter_Hash_IsLadderClimbingOffBottom, character.IsLadderClimbingOffBottom);
            animator.SetFloat(Animator_Parameter_Hash_LadderClimbingDirection, character.LadderClimbingDirection);

            float Velocity = animator.GetFloat(Animation_Curve_Hash_Velocity_Attack_Forward);
            character.Controller.AddVelocity(character.transform.forward * Velocity);
            /*
            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;
            */
        }

        private void UpdateLocomotionParameters()
        {
            float speedRatio = 0f;
            if (character.MovementSettings.HasRunState)
            {
                // 걷기 상태일때는 Animator_Threshold_Walk값에, 뜀 상태일때는 Animator_Threshold_Run값에 맞춰준다.
                if (character.Controller.HorizontalSpeed <= character.MovementSettings.WalkSpeed)
                {
                    speedRatio = Animator_Threshold_Walk * (character.Controller.HorizontalSpeed / character.MovementSettings.WalkSpeed);
                }
                else
                {
                    speedRatio = Animator_Threshold_Walk +
                        (character.Controller.HorizontalSpeed - character.MovementSettings.WalkSpeed) / (character.MovementSettings.RunSpeed - character.MovementSettings.WalkSpeed) *
                        (Animator_Threshold_Sprint - Animator_Threshold_Walk);
                }
            }
            else
            {
                speedRatio = character.Controller.HorizontalSpeed / character.MovementSettings.WalkSpeed;
            }
            animator.SetFloat(Animator_Parameter_Hash_Speed, speedRatio);

            // TODO: 이속 빨라지거나 느려지면(ex: 디버프) 로코모션 재생속도 증감
            animator.SetFloat(Animator_Parameter_Hash_MotionSpeed, 1f);

            // Calculate the dot product of the character's viewing direction and movement direction
            // to set the Forward and Right values. (for strafing animations)
            // TODO: Calculate when strafing is needed (ex: camera lock on target)
            if (character.Controller.UseDirectionalMovement)
            {
                Vector3 normalizedVelocity = character.Controller.Velocity.normalized;
                float forward = Vector3.Dot(character.Controller.Forward, normalizedVelocity);
                float right = Vector3.Dot(character.Controller.Right, normalizedVelocity);
                animator.SetFloat(Animator_Parameter_Hash_Forward, forward);
                animator.SetFloat(Animator_Parameter_Hash_Right, right);
            }
        }

        private void OnAnimatorMove()
        {
            if (character.Controller.UseRootMotion)
            {
                character.Controller.RootMotionPositionDelta += animator.deltaPosition;
                character.Controller.RootMotionRotationDelta = animator.deltaRotation * character.Controller.RootMotionRotationDelta;
            }
        }

        private void OnAnimatorIK(int layerIndex)
        {

        }
    }
}