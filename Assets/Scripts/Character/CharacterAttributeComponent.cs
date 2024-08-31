using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class CharacterAttributeComponent : MonoBehaviour
    {
        public List<CharacterAttribute> characterAttributes = new List<CharacterAttribute>();

        private Character character;
        private Dictionary<ECharacterAttribute, CharacterAttribute> attributes;


        private void Awake()
        {
            character = GetComponent<Character>();

            foreach(var characterAttribute in characterAttributes)
            {
                attributes.Add(characterAttribute.attribute, characterAttribute);
            }
        }
    }
}