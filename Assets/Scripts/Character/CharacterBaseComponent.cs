using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Character))]
    public class CharacterBaseComponent : MonoBehaviour
    {
        protected Character character;

        protected virtual void Awake()
        {
            character = GetComponent<Character>();
        }
    }
}