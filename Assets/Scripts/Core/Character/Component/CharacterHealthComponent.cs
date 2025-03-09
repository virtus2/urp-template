using System;
using UnityEngine;

namespace Core.Character.Component
{
    [RequireComponent(typeof(BaseCharacter))]
    public class CharacterHealthComponent : MonoBehaviour
    {
        public float MaxHealth;
        public float CurrentHealth { get; private set; }
        public Action<float> OnHealthValueChanged;

        private BaseCharacter character;

        private void Awake()
        {
            character = GetComponent<BaseCharacter>();
        }

        private void Start()
        {
            SetCurrentHealth(MaxHealth);
        }

        public void SetCurrentHealth(float health)
        {
            CurrentHealth = health;
            OnHealthValueChanged?.Invoke(CurrentHealth);
        }

        public void AddCurrentHealth(float health)
        {
            CurrentHealth += health;
            OnHealthValueChanged?.Invoke(CurrentHealth);
        }
    }
}