using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class CharacterHealthComponent : CharacterBaseComponent
    {
        public float MaxHealth;
        public float CurrentHealth { get; private set; }
        public Action<float> OnHealthValueChanged;

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