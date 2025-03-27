using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemImage : MonoBehaviour, IPointerDownHandler
{
    public Image Image;
    public RectInt ItemRect;
    public Action<InventoryItemImage> OnPointerDownAction;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = transform as RectTransform;
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

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPointerDownAction?.Invoke(this);
    }
}
