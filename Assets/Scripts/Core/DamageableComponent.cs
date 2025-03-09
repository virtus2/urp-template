using UnityEngine;

namespace Core
{

    public abstract class DamageableComponent : MonoBehaviour
    {
        public bool CanTakeDamage;
        public abstract void TakeDamage(float damage = 0f);
    }
}
