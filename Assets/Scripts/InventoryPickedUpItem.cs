using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryPickedUpItem : MonoBehaviour
{
    public Image Image;
    public RectInt ItemRect;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = transform as RectTransform;
    }

    private void Update()
    {
        Vector2 pointerPosition = Pointer.current.position.ReadValue();
        transform.position = pointerPosition;
    }
    public void SetPositionAndSize(RectInt rect, Vector2Int cellSize, Vector2Int cellGap)
    {
        ItemRect = rect;

        rectTransform.anchoredPosition = new Vector2(
            rect.position.x * cellSize.x + rect.position.x * cellGap.x,
            -rect.position.y * cellSize.y - rect.position.y * cellGap.y
        );

        rectTransform.sizeDelta = new Vector2(
            rect.size.x * cellSize.x + (rect.size.x - 1) * cellGap.x,
            rect.size.y * cellSize.y + (rect.size.y - 1) * cellGap.y
        );
    }
}
