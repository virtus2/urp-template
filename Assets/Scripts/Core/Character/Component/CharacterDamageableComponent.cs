using UnityEngine;

namespace Core.Character.Component
{
    [RequireComponent(typeof(BaseCharacter))]
    [RequireComponent(typeof(CharacterHealthComponent))]
    public class CharacterDamageableComponent : DamageableComponent
    {
        private BaseCharacter character;
        private CharacterHealthComponent health;

        private void Awake()
        {
            character = GetComponent<BaseCharacter>();
            health = GetComponent<CharacterHealthComponent>();
        }

        public override void TakeDamage(float damage = 0)
        {
            health.AddCurrentHealth(-damage);
        }
    }
}