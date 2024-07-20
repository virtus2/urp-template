using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [System.Serializable]
    public class MovementSettings
    {
        public float Acceleration = 25.0f; // In meters/second
        public float Decceleration = 25.0f; // In meters/second
        public float MaxHorizontalSpeed = 8.0f; // In meters/second
        public float JumpSpeed = 10.0f; // In meters/second
        public float JumpAbortSpeed = 10.0f; // In meters/second
    }

    public class Character : MonoBehaviour
    { 
        public MovementSettings movementSettings;
        
        public Controller controller;
        public CharacterStateMachine stateMachine;

        public void SetController(Controller controller)
        {
            this.controller = controller;
        }

        private void FixedUpdate()
        {
            
        }
    }
}
