using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryCell : MonoBehaviour, IPointerDownHandler
{
    public Action OnLeftMouseButtonDown;
    public Action OnRightMouseButtonDown;
    public Vector2Int GridPosition;

    public Vector2 TopLeft;
    public Vector2 TopRight;
    public Vector2 BottomLeft;
    public Vector2 BottomRight;

    public void SetCornerPositions()
    {
        TopLeft = new Vector2(transform.localPosition.x, transform.localPosition.y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            OnLeftMouseButtonDown?.Invoke();
        else if (eventData.button == PointerEventData.InputButton.Right)
            OnRightMouseButtonDown?.Invoke();

        Debug.Log(eventData);
    }
}
