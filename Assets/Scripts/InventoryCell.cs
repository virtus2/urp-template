using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryCell : MonoBehaviour, IPointerDownHandler
{
    public Action OnLeftMouseButtonDown;
    public Action OnRightMouseButtonDown;
    public Vector2 GridPosition;


    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            OnLeftMouseButtonDown?.Invoke();
        else if (eventData.button == PointerEventData.InputButton.Right)
            OnRightMouseButtonDown?.Invoke();

        Debug.Log(eventData);
    }
}
