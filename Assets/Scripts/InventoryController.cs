using Unity.AppUI.UI;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
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
public class InventoryController : MonoBehaviour, IPointerMoveHandler, IPointerDownHandler, IPointerExitHandler
{
    public EInventoryControlState InventoryControlState = EInventoryControlState.None;
    public Vector2Int CurrentGridPositionOfPointer;
    
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
        inventoryView.AddInventoryItemImage(entry);
    }

    private void HandleOnItemRemoved(InventoryItemEntry entry)
    {
        inventoryView.RemoveInventoryItemImage(entry);
    }

    private Vector2Int ScreenToGrid(Vector2 screenPoint)
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

        Vector2Int gridPosition = new Vector2Int(colIndex, inventory.Height - rowIndex - 1);
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
        Vector2Int gridPosition = ScreenToGrid(eventData.position);
        bool inside = inventory.IsInsideInventory(gridPosition);

        if (inside == false)
            return;
        
        if (CurrentGridPositionOfPointer == gridPosition)
            return;

        CurrentGridPositionOfPointer = gridPosition;
        inventoryView.ClearCellColor();

        switch (InventoryControlState)
        {
            case EInventoryControlState.None:
                {
                    if (inventory.TryGetItemAt(gridPosition, out InventoryItemEntry entry))
                    {
                        inventoryView.SetCellColor(entry.Rect.position, entry.Size, Color.yellow);
                    }
                }
                break;
            case EInventoryControlState.ItemPickedUp:
                {
                    Vector2Int position = gridPosition - inventory.PickedUpItem.Size + Vector2Int.one;
                    RectInt rect = new RectInt(position, inventory.PickedUpItem.Size);
                    if (inventory.IsInsideInventory(rect) == false)
                        return;

                    if (inventory.IsFitInInventory(rect))
                    {
                        inventoryView.SetCellColor(position, inventory.PickedUpItem.Size, Color.green);
                    }
                    else
                    {
                        inventoryView.SetCellColor(position, inventory.PickedUpItem.Size, Color.red);
                    }
                }

                break;
        }

        CurrentGridPositionOfPointer = gridPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2Int gridPosition = ScreenToGrid(eventData.position);
        bool inside = inventory.IsInsideInventory(gridPosition);
        if (inside == false)
            return;

        switch (InventoryControlState)
        {
            case EInventoryControlState.None:
                {
                    if (inventory.TryGetItemAt(gridPosition, out InventoryItemEntry entry))
                    {
                        if (inventory.TryRemoveItem(entry))
                        {
                            InventoryControlState = EInventoryControlState.ItemPickedUp;
                            inventoryView.ShowPickedUpItem(entry);
                            inventory.PickedUpItem = entry;

                            RectInt rect = new RectInt(gridPosition - entry.Size + Vector2Int.one, entry.Size);
                            if (inventory.IsFitInInventory(rect))
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
                    Vector2Int position = gridPosition - inventory.PickedUpItem.Size + Vector2Int.one;
                    if (inventory.TryAddItem(inventory.PickedUpItem, position))
                    {
                        InventoryControlState = EInventoryControlState.None;
                        inventoryView.HidePickedUpItem();
                        inventoryView.SetCellColor(position, inventory.PickedUpItem.Size, Color.yellow);
                        inventory.PickedUpItem = null;
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
