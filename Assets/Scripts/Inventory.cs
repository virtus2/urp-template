using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface InventoryItemEntry
{
    public Vector2Int Size { get; }

}

public enum EInventoryContrlState
{
    None,
    ItemPickedUp,
    UsingItemToOneTargetItem, 
    UsingItemToTwoTargetItem, // If we dont need this, just remove. Think about POE Awakener's orb.
}

[AddComponentMenu("Arpg/Inventory")]
public class Inventory : MonoBehaviour
{
    [Range(1, 50)]
    public int Height;

    [Range(1, 50)]
    public int Width;

    public Vector2 CellGap;
    public Vector2 CellSize;
    public InventoryCell InventoryCellPrefab;

    [Header("Any values in GridLayoutGroup will automatically modified by the Inventory.")]
    public GridLayoutGroup GridLayoutGroup;

    private List<InventoryCell> inventoryCellList = new List<InventoryCell>();
    private Dictionary<Vector2Int, InventoryCell> inventoryCellByPosition = new Dictionary<Vector2Int, InventoryCell>(); // [y, x]


    public void OnValidate()
    {
        GridLayoutGroup.constraintCount = Width;

        if (inventoryCellByPosition != null && inventoryCellByPosition.Count > 0)
        {
            foreach (KeyValuePair<Vector2Int, InventoryCell> kvp in inventoryCellByPosition)
            {
                Vector2Int position = kvp.Key;
                InventoryCell cell = kvp.Value;
                cell.GridPosition = position;
            }
        }
    }

    [NaughtyAttributes.Button]
    public void InstantiateCells()
    {
        if (InventoryCellPrefab == null)
            return;

        InventoryCell[] cells = transform.GetComponentsInChildren<InventoryCell>();
        foreach (InventoryCell cell in cells)
        {
            DestroyImmediate(cell.gameObject);
        }
        inventoryCellList.Clear();
        inventoryCellByPosition.Clear();

        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                InventoryCell cell = Instantiate(InventoryCellPrefab, transform);
                Vector2Int position = new Vector2Int(i, j);
                inventoryCellList.Add(cell);
                inventoryCellByPosition.Add(position, cell);

                cell.GridPosition = position;
            }
        }
    }

    public bool TryAddItemToEmptySpace(InventoryItemEntry entry)
    {
        if (TryGetEmptySpace(entry.Size, out RectInt emptySpaceRect) == false)
            return false;

        
    }

    public bool TryGetEmptySpace(Vector2Int size, out RectInt emptySpaceRect)
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Vector2Int position = new Vector2Int(x, y);
                RectInt rect = new RectInt(position, size);

                if (!IsFitInInventory(rect))
                {
                    continue;
                }

                emptySpaceRect = rect;
                return true;
            }
        }

        emptySpaceRect = new RectInt();
        return false;
    }

    //
    // Rect Utilities
    //
    private bool IsInsideInventory(RectInt rect)
    {
        if (rect.xMax > Width || rect.yMax > Height) return false;
        if (rect.xMin < 0 || rect.yMin < 0) return false;
        return true;
    }
    private bool IsFitInInventory(RectInt rect)
    {
        if (!IsInsideInventory(rect))
        {
            return false;
        }
        foreach (var item in inventoryGameItems)
        {
            if (item.Value.rect.Overlaps(rect))
            {
                return false;
            }
        }
        return true;
    }
}
