using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InventoryItemEntry
{
    public Vector2Int Size;
    public RectInt Rect;
}

[AddComponentMenu("Arpg/Inventory")]
public class Inventory : MonoBehaviour
{
    [Header("Inventory Grid")]
    [Range(1, 50)]
    public int Height;

    [Range(1, 50)]
    public int Width;

    public Action<InventoryItemEntry> OnItemAdded;
    public Action<InventoryItemEntry> OnItemRemoved;

    private Dictionary<InventoryItemEntry, RectInt> inventoryItems = new Dictionary<InventoryItemEntry, RectInt>();

    public bool TryAddItem(InventoryItemEntry entry)
    {
        if (TryGetEmptySpace(entry.Size, out RectInt emptySpaceRect) == false)
            return false;

        inventoryItems.Add(entry, emptySpaceRect);
        entry.Rect = emptySpaceRect;
        entry.Size = emptySpaceRect.size;

        OnItemAdded?.Invoke(entry);
        return true;
    }

    public bool TryRemoveItem(InventoryItemEntry entry)
    {
        bool found = inventoryItems.ContainsKey(entry);
        if (found)
        {
            inventoryItems.Remove(entry);
            OnItemRemoved?.Invoke(entry);
            return true;
        }
        return false;
    }

    public bool TryGetItemAt(Vector2Int position, out InventoryItemEntry entry)
    {
        foreach (var item in inventoryItems)
        {
            if (item.Value.Contains(position))
            {
                entry = item.Key;
                return true;
            }
        }
        entry = null;
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
    public bool IsInsideInventory(Vector2Int gridPosition)
    {
        return gridPosition.x >= 0 && gridPosition.x < Width && gridPosition.y >= 0 && gridPosition.y < Height;
    }
    public bool IsInsideInventory(RectInt rect)
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

    [NaughtyAttributes.Button]
    public void TestAddItem()
    {
        InventoryItemEntry testItemEntry = new InventoryItemEntry();
        testItemEntry.Size = new Vector2Int(Random.Range(1, 3), Random.Range(1, 3));
        bool added = TryAddItem(testItemEntry);

    }

    public void TestRemoveItem()
    {

    }
#endif
}
