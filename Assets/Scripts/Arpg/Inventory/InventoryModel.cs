using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryModel
{
    public Action<InventoryItemEntry> OnItemAdded;
    public Action<InventoryItemEntry> OnItemRemoved;

    private Dictionary<InventoryItemEntry, RectInt> inventoryItems;
    private int inventoryHeight;
    private int inventoryWidth;

    public InventoryModel(int height, int width)
    {
        inventoryItems = new Dictionary<InventoryItemEntry, RectInt>();
        inventoryHeight = height;
        inventoryWidth = width;
    }

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

    public bool TryAddItem(InventoryItemEntry entry, Vector2Int gridPosition)
    {
        RectInt rect = new RectInt(gridPosition, entry.Size);
        if (IsFitInInventory(rect) == false)
            return false;

        inventoryItems.Add(entry, rect);
        entry.Rect = rect;
        entry.Size = rect.size;

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
        for (int y = 0; y < inventoryHeight; y++)
        {
            for (int x = 0; x < inventoryWidth; x++)
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
        return gridPosition.x >= 0 && gridPosition.x < inventoryWidth && gridPosition.y >= 0 && gridPosition.y < inventoryHeight;
    }
    public bool IsInsideInventory(RectInt rect)
    {
        if (rect.xMax > inventoryWidth || rect.yMax > inventoryHeight) return false;
        if (rect.xMin < 0 || rect.yMin < 0) return false;
        return true;
    }

    public bool IsFitInInventory(RectInt rect)
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

}
