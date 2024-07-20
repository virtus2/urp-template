using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerInputHandler inputHandler;
    private PlayerCharacterController characterController;
    private Character character;

    public Character testCharacter;
    
    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        characterController = GetComponent<PlayerCharacterController>();

        OnPlayerCharacterSpawned(testCharacter);
    }

    private void OnPlayerCharacterSpawned(Character character)
    {
        this.character = character;
        character.SetController(characterController);
    }
}
