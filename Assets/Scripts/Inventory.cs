using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class InventoryItemEntry
{
    public Vector2Int Size;
}

public enum EInventoryControlState
{
    None,
    ItemPickedUp,
    UsingItemToOneTargetItem, 
    UsingItemToTwoTargetItem, // If we dont need this, just remove. Think about POE Awakener's orb.
}

[AddComponentMenu("Arpg/Inventory")]
public class Inventory : MonoBehaviour
{
    [Header("Inventory Grid")]
    [Range(1, 50)]
    public int Height;

    [Range(1, 50)]
    public int Width;

    public Vector2Int CellSize = new Vector2Int(100, 100);

    public Vector2Int CellGap = new Vector2Int(5, 5);
    
    public Transform InventoryGrid;
    public InventoryCell InventoryCellPrefab;

    [Header("Any values in GridLayoutGroup will automatically modified by the Inventory.")]
    public GridLayoutGroup GridLayoutGroup;

    [Header("Inventory Items")]
    public InventoryItem InventoryItemPrefab;
    public Transform InventoryItemParent;
    private ObjectPool<InventoryItem> inventoryItemPool;


    private List<InventoryCell> inventoryCellList = new List<InventoryCell>();
    private Dictionary<Vector2Int, InventoryCell> inventoryCellByPosition = new Dictionary<Vector2Int, InventoryCell>(); // [y, x]
    private Dictionary<InventoryItemEntry, RectInt> inventoryItems = new Dictionary<InventoryItemEntry, RectInt>();

    private void Awake()
    {
        inventoryItemPool = new ObjectPool<InventoryItem>(
            createFunc: CreateInventoryItem,
            defaultCapacity: Height * Width,
            maxSize: Height * Width * 2
        );
    }

    private InventoryItem CreateInventoryItem()
    {
        InventoryItem item = Instantiate(InventoryItemPrefab, InventoryItemParent);
        return item;
    }

    public bool TryAddItemToEmptySpace(InventoryItemEntry entry)
    {
        if (TryGetEmptySpace(entry.Size, out RectInt emptySpaceRect) == false)
            return false;

        inventoryItems.Add(entry, emptySpaceRect);
        InventoryItem item = inventoryItemPool.Get();
        item.SetPositionAndSize(emptySpaceRect, CellSize, CellGap);
        return true;
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
        foreach (var item in inventoryItems)
        {
            if (item.Value.Overlaps(rect))
            {
                return false;
            }
        }
        return true;
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        GridLayoutGroup.cellSize = CellSize;
        GridLayoutGroup.spacing = CellGap;
        GridLayoutGroup.constraintCount = Width;
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
                InventoryCell cell = Instantiate(InventoryCellPrefab, InventoryGrid.transform);
                Vector2Int position = new Vector2Int(i, j);
                inventoryCellList.Add(cell);
                inventoryCellByPosition.Add(position, cell);
                cell.GridPosition = position;
            }
        }
    }

    [NaughtyAttributes.Button]
    public void SetCellPositions()
    {
        InventoryCell[] cells = transform.GetComponentsInChildren<InventoryCell>();
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].GridPosition = new Vector2Int(i / Width, i%Width);
        }
    }

    [NaughtyAttributes.Button]
    public void TestAddItem()
    {
        InventoryItemEntry testItemEntry = new InventoryItemEntry();
        testItemEntry.Size = new Vector2Int(Random.Range(1, 3), Random.Range(1, 3));
        bool added = TryAddItemToEmptySpace(testItemEntry);

        if (added)
        {
        }
        


    }

    public void TestRemoveItem()
    {

    }
#endif
}
