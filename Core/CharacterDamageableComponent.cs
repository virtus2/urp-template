using Core.Character.Component;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(CharacterHealthComponent))]
    public class CharacterDamageableComponent : DamageableComponent
    {
        private Character character;
        private CharacterHealthComponent health;

        private void Awake()
        {
            character = GetComponent<Character>();
            health = GetComponent<CharacterHealthComponent>();
        }

        public override void TakeDamage(float damage = 0)
        {
            health.AddCurrentHealth(-damage);
        }
    }
}