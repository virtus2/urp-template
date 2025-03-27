using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public enum EInventoryControlState
{
    None,
    ItemPickedUp,
    UsingItemToOneTargetItem,
    UsingItemToTwoTargetItem, // If we dont need this, just remove. Think about POE Awakener's orb.
}

[RequireComponent(typeof(InventoryView))]
public class InventoryController : MonoBehaviour, IPointerMoveHandler, IPointerDownHandler
{
    public EInventoryControlState InventoryControlState = EInventoryControlState.None;
    
    private Inventory inventory;
    private InventoryView inventoryView;

    private void OnEnable()
    {
        inventory.OnItemAdded += HandleOnItemAdded;
        inventory.OnItemRemoved += HandleOnItemRemoved;
    }

    private void OnDisable()
    {
        inventory.OnItemAdded -= HandleOnItemAdded;
        inventory.OnItemRemoved -= HandleOnItemRemoved;
    }

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        inventoryView = GetComponent<InventoryView>();
    }

    private void Update()
    {
        switch (InventoryControlState)
        {
            case EInventoryControlState.ItemPickedUp:

                break;
            default:
                break;
        }
    }

    private void HandleOnItemAdded(InventoryItemEntry entry)
    {
        inventoryView.AddInventoryItemImage(entry, OnPointerDown_InventoryItemImage);
    }
    private void HandleOnItemRemoved(InventoryItemEntry entry)
    {
        inventoryView.RemoveInventoryItemImage(entry);
    }

    private void OnPointerDown_InventoryItemImage(InventoryItemImage itemImage)
    {
        switch(InventoryControlState)
        {
            case EInventoryControlState.None:
                bool found = inventory.TryGetItemAt(itemImage.ItemRect.position, out InventoryItemEntry entry);
                if (false)
                {
                    InventoryControlState = EInventoryControlState.ItemPickedUp;
                    inventoryView.ShowPickedUpItem(entry);
                    inventory.TryRemoveItem(entry);
                }
                

                break;
            case EInventoryControlState.ItemPickedUp:

                break;
        }
    }

    internal Vector2Int ScreenToGrid(Vector2 screenPoint)
    {
        // Top left is (0,0) in Grid
        var pos = ScreenToLocalPositionInRenderer(screenPoint);
        var sizeDelta = ((RectTransform)inventoryView.transform).sizeDelta;
        pos.x += sizeDelta.x / 2;
        pos.y += sizeDelta.y / 2;

        float cellWidth = inventoryView.CellSize.x + inventoryView.CellGap.x;
        float cellHeight = inventoryView.CellSize.y + inventoryView.CellGap.y;

        int colIndex = Mathf.FloorToInt(pos.x / cellWidth);
        int rowIndex = Mathf.FloorToInt(pos.y / cellHeight);

        Vector2Int gridPosition = new Vector2Int(inventory.Height - rowIndex - 1, colIndex);
        return gridPosition;
    }

    private Vector2 ScreenToLocalPositionInRenderer(Vector2 screenPosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)inventoryView.transform,
            screenPosition,
            null, // canvas render mode is screen space overlay
            out var localPosition
        );
        return localPosition;
    }

    public void OnPointerMove(PointerEventData eventData)
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // TODO: 다른 오브젝트에 ray cast 막혀서 호출이 안됨
        Vector2Int gridPosition = ScreenToGrid(eventData.position);
        bool inside = inventory.IsInsideInventory(gridPosition);
        if (inside)
        {
            if (inventory.TryGetItemAt(gridPosition, out InventoryItemEntry entry))
            {
                Debug.Log(entry);
            }

        }
    }
}
