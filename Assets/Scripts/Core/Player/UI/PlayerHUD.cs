using Core.Character;
using Core.Character.Component;
using UnityEngine;

namespace Core.Player.UI
{
    /// <summary>
    /// �÷��̾��� ��ũ���� ǥ�õǴ� Screen space overlay UI
    /// </summary>
    public class PlayerHUD : MonoBehaviour
    {
        private BaseCharacter playerCharacter;
        private CharacterHealthComponent healthComponent;

        private void Awake()
        {
            PlayerInstance.Instance.OnPlayerCharacterSpawned += SetCharacter;
        }

        private void SetCharacter(BaseCharacter character)
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
