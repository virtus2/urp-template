using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Character))]
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