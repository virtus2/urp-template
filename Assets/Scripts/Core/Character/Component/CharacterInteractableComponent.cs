using System;
using UnityEngine;

namespace Core.Character.Component
{
    [RequireComponent(typeof(BaseCharacter))]
    public class CharacterInteractableComponent : InteractableComponent
    {
        public override void BeginInteract()
        {
            throw new NotImplementedException();
        }

        public override void EndInteract()
        {
            throw new NotImplementedException();
        }

        public override void Interact()
        {
            throw new NotImplementedException();
        }
    }
}