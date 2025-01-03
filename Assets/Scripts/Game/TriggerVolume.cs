using Core;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerVolume : MonoBehaviour
{
    protected BoxCollider boxCollider;

    protected virtual void OnTriggerEnter(Collider other) { }
    protected virtual void OnTriggerExit(Collider other) { }
}
