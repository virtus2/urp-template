using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class Player : MonoBehaviour
    {
        private PlayerInputHandler inputHandler;
        private PlayerCharacterController characterController;
        private Camera playerCamera; // TODO: �ó׸ӽ� ī�޶� ��� �ٷ���� �˾ƺ��� �����ϱ�

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
            characterController.SetCamera(playerCamera);
            characterController.SetCharacter(character);
            character.SetController(characterController);
        }
    }
}