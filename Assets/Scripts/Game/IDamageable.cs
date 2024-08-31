using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public interface IDamageable
    {
        bool CanTakeDamage();
        void TakeDamage(float damage);
    }
}