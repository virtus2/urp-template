using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(CharacterController))]
    public abstract class Controller : MonoBehaviour
    {
        protected Character character;
        protected CharacterController characterController; // Unity's CharacterController Component

        public virtual void Init(Character character)
        {
            this.character = character;
            characterController = GetComponent<CharacterController>();
        }
    }
}
