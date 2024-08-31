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
        public Action<float> OnHealthValueChanged;

        [ShowNonSerializedField]
        private float currentHealth;

        private void Start()
        {
            SetCurrentHealth(MaxHealth);
        }

        public void SetCurrentHealth(float health)
        {
            currentHealth = health;
            OnHealthValueChanged?.Invoke(currentHealth);
        }

        public void AddCurrentHealth(float health)
        {
            currentHealth += health;
            OnHealthValueChanged?.Invoke(currentHealth);
        }
    }
}