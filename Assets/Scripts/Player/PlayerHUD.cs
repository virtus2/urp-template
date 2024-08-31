using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    /// <summary>
    /// �÷��̾��� ��ũ���� ǥ�õǴ� Screen space overlay UI
    /// </summary>
    public class PlayerHUD : MonoBehaviour
    {
        private Character playerCharacter;
        private CharacterHealthComponent healthComponent;

        private void Awake()
        {
            Player.Instance.OnPlayerCharacterSpawned += SetCharacter;
        }

        private void SetCharacter(Character character)
        {
            playerCharacter = character;

            healthComponent = playerCharacter.GetComponent<CharacterHealthComponent>();
            if (healthComponent)
            {
                healthComponent.OnHealthValueChanged += UpdateHealthBar;
            }
        }

        private void UpdateHealthBar(float currentHealth)
        {
            Debug.Log($"CurrentHealth: {currentHealth}");
        }
    }
}
