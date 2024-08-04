using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class Player : MonoBehaviour
    {
        public Character PlayerCharacter { get; private set; }
        private PlayerInputHandler inputHandler;
        private PlayerCharacterController characterController;
        private Camera playerCamera; // TODO: 시네머신 카메라 어떻게 다루는지 알아보고 변경하기

        public Character testCharacter;

        private void Awake()
        {
            inputHandler = GetComponent<PlayerInputHandler>();
            characterController = GetComponent<PlayerCharacterController>();
            playerCamera = Camera.main;

            OnPlayerCharacterSpawned(testCharacter);
        }

        private void OnPlayerCharacterSpawned(Character character)
        {
            PlayerCharacter = character;

            characterController.SetCamera(playerCamera);
            characterController.SetCharacter(character);

            character.SetController(characterController);
        }
    }
}