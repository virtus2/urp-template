using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryPresenter
{
    public Vector2Int CurrentGridPositionOfPointer;

    private InventoryModel inventoryModel;
    private InventoryView inventoryView;
    private RectTransform inventoryRectTransform;
    private IItemPickUpHandler itemPickUpHandler;
    private IItemDropHandler itemDropHandler;

    private int inventoryHeight;
    private int inventoryWidth;
    private Vector2Int inventoryCellSize;
    private Vector2Int inventoryCellGap;

    private InventoryItemEntry currentPickedUpItem;

    public InventoryPresenter(InventoryModel model, InventoryView view, IItemPickUpHandler pickUpHandler, IItemDropHandler dropHandler, RectTransform inventoryRect, int height, int width, Vector2Int cellSize, Vector2Int cellGap)
    {
        inventoryModel = model;
        inventoryModel.OnItemAdded += HandleOnItemAdded;
        inventoryModel.OnItemRemoved += HandleOnItemRemoved;

        inventoryView = view;
        inventoryRectTransform = inventoryRect;
        itemPickUpHandler = pickUpHandler;
        itemDropHandler = dropHandler;
        inventoryHeight = height;
        inventoryWidth = width;
        inventoryCellSize = cellSize;
        inventoryCellGap = cellGap;
    }

    private void HandleOnItemAdded(InventoryItemEntry entry)
    {
        inventoryView.AddInventoryItemImage(entry);
    }

    private void HandleOnItemRemoved(InventoryItemEntry entry)
    {
        inventoryView.RemoveInventoryItemImage(entry);
    }

    public void HandleItemPickedUp(InventoryItemEntry entry)
    {
        currentPickedUpItem = entry;
    }

    public void HandleItemDropped()
    {
        currentPickedUpItem = null;
    }

    private Vector2Int ScreenToGrid(Vector2 screenPoint)
    {
        // Top left is (0,0) in Grid
        var pos = ScreenToLocalPositionInRenderer(screenPoint);
        var sizeDelta = inventoryRectTransform.sizeDelta;
        pos.x += sizeDelta.x / 2;
        pos.y += sizeDelta.y / 2;

        float cellWidth = inventoryCellSize.x + inventoryCellGap.x;
        float cellHeight = inventoryCellSize.y + inventoryCellGap.y;

        int colIndex = Mathf.FloorToInt(pos.x / cellWidth);
        int rowIndex = Mathf.FloorToInt(pos.y / cellHeight);

        Vector2Int gridPosition = new Vector2Int(colIndex, inventoryHeight - rowIndex - 1);
        return gridPosition;
    }

    private Vector2 ScreenToLocalPositionInRenderer(Vector2 screenPosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            inventoryRectTransform,
            screenPosition,
            null, // canvas render mode is screen space overlay
            out var localPosition
        );
        return localPosition;
    }

    public void OnPointerMove(PointerEventData eventData, EInventoryControlState state)
    {
        Vector2Int gridPosition = ScreenToGrid(eventData.position);
        bool inside = inventoryModel.IsInsideInventory(gridPosition);

        Debug.Log(gridPosition);
        if (inside == false)
            return;

        if (CurrentGridPositionOfPointer == gridPosition)
            return;

        CurrentGridPositionOfPointer = gridPosition;
        inventoryView.ClearCellColor();

        switch (state)
        {
            case EInventoryControlState.None:
                {
                    if (inventoryModel.TryGetItemAt(gridPosition, out InventoryItemEntry entry))
                    {
                        inventoryView.SetCellColor(entry.Rect.position, entry.Size, Color.yellow);
                    }
                }
                break;
            case EInventoryControlState.ItemPickedUp:
                {
                    Vector2Int position = gridPosition - currentPickedUpItem.Size + Vector2Int.one;
                    RectInt rect = new RectInt(position, currentPickedUpItem.Size);

                    if (inventoryModel.IsFitInInventory(rect))
                    {
                        inventoryView.SetCellColor(position, currentPickedUpItem.Size, Color.green);
                    }
                    else
                    {
                        inventoryView.SetCellColor(position, currentPickedUpItem.Size, Color.red);
                    }
                }

                break;
        }

        CurrentGridPositionOfPointer = gridPosition;
    }

    public void OnPointerDown(PointerEventData eventData, EInventoryControlState state)
    {
        Vector2Int gridPosition = ScreenToGrid(eventData.position);
        bool inside = inventoryModel.IsInsideInventory(gridPosition);
        if (inside == false)
            return;

        switch (state)
        {
            case EInventoryControlState.None:
                {
                    if (inventoryModel.TryGetItemAt(gridPosition, out InventoryItemEntry entry))
                    {
                        if (inventoryModel.TryRemoveItem(entry))
                        {
                            if(itemPickUpHandler != null)
                                itemPickUpHandler.PickUpItem(entry);

                            RectInt rect = new RectInt(gridPosition - entry.Size + Vector2Int.one, entry.Size);
                            if (inventoryModel.IsFitInInventory(rect))
                            {
                                inventoryView.SetCellColor(rect.position, rect.size, Color.green);
                            }
                            else
                            {
                                inventoryView.SetCellColor(rect.position, rect.size, Color.white);
                            }
                        }
                    }
                }

                break;
            case EInventoryControlState.ItemPickedUp:
                {
                    Vector2Int position = gridPosition - currentPickedUpItem.Size + Vector2Int.one;
                    if (inventoryModel.TryAddItem(currentPickedUpItem, position))
                    {
                        inventoryView.SetCellColor(position, currentPickedUpItem.Size, Color.yellow);
                        itemDropHandler.DropPickedUpItem();
                    }
                }

                break;

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryView.ClearCellColor();
        CurrentGridPositionOfPointer = new Vector2Int(-1, -1);
    }
}
