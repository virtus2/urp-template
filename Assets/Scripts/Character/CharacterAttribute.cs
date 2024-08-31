using System;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Default Character Attribute", menuName = "Scriptable Objects/Character/New Attribute")]
    public class CharacterAttribute : ScriptableObject
    {
        public ECharacterAttribute attribute;

        public Action OnAttributeValueChanged;


    }
}
