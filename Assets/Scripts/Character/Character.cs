using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [System.Serializable]
    public class MovementSettings
    {
        public float WalkingSpeed = 0.83f;
        public float RunningSpeed= 1.25f;
        public float JumpSpeed = 10.0f; // In meters/second
        public float JumpAbortSpeed = 10.0f; // In meters/second
    }

    public class Character : MonoBehaviour
    {
        public MovementSettings movementSettings;
        public Vector3 Velocity => controller.Velocity;
        public bool IsMoving => stateMachine.CurrentState == CharacterState.GroundMove;

        private CharacterStateMachine stateMachine;
        private Controller controller;
        private Animator animator;

        private void Awake()
        {
            stateMachine = GetComponent<CharacterStateMachine>();
            animator = GetComponent<Animator>();
        }

        public void SetController(Controller controller)
        {
            this.controller = controller;
        }
    }
}
