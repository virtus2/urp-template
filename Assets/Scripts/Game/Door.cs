using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Door : MonoBehaviour, Core.IInteractable
    {
        public EInteractType Type => throw new System.NotImplementedException();

        public float HoldDuration => throw new System.NotImplementedException();

        public bool NeedToShowWidget()
        {
            return true;
        }
        public bool IsInteractable()
        {
            return true;
        }
        public void BeginInteract()
        {
            Debug.Log("BeginInteract!");
        }
        public void Interact()
        {
            Debug.Log("Interact!");
        }
        public void EndInteract()
        {
            Debug.Log("EndInteract!");
        }
    }
}