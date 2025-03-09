using Core;
using UnityEngine;

namespace Core.Level.Placeable
{
    [RequireComponent(typeof(BoxCollider))]
    public class TriggerVolume : MonoBehaviour
    {
        protected BoxCollider boxCollider;

        protected virtual void OnTriggerEnter(Collider other) { }
        protected virtual void OnTriggerExit(Collider other) { }
    }
}