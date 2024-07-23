using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

namespace Core
{
    public class PlayerCharacterController : Controller
    {
        private PlayerInputHandler inputHandler;
        private Character playerCharacter;
        private Camera playerCamera;

        private void Awake()
        {
            inputHandler = GetComponent<PlayerInputHandler>();
        }

        public void SetCamera(Camera camera)
        {
            playerCamera = camera;
        }

        protected override void Update()
        {
            Vector2 playerInput = inputHandler.MoveInput;
            playerInput.Normalize();
            MovementInput = playerInput;

            RollPressed = inputHandler.RollInput;
        }
    }
}