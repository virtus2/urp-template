using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class InventoryView
{
    private int inventoryHeight;
    private int inventoryWidth;

    private Vector2Int inventoryCellSize;
    private Vector2Int inventoryCellGap;

    private List<InventoryCell> inventoryCells;
    private Dictionary<Vector2Int, InventoryCell> inventoryCellsByPosition;

    private ObjectPool<InventoryItemImage> imagePool;
    private Dictionary<InventoryItemEntry, InventoryItemImage> inventoryItemImages;

    public InventoryView(int height, int width, Vector2Int cellSize, Vector2Int cellGap, InventoryCell[] cells, ObjectPool<InventoryItemImage> pool)
    {
        inventoryHeight = height;
        inventoryWidth = width;

        inventoryCellSize = cellSize;
        inventoryCellGap = cellGap;

        inventoryCells = new List<InventoryCell>();
        inventoryCellsByPosition = new Dictionary<Vector2Int, InventoryCell>();
        for (int i = 0; i < cells.Length; i++)
        {
            Vector2Int gridPosition = new Vector2Int(i % inventoryWidth, i / inventoryWidth);
            cells[i].GridPosition = gridPosition;
            inventoryCells.Add(cells[i]);
            inventoryCellsByPosition.Add(gridPosition, cells[i]);
        }

        imagePool = pool;
        inventoryItemImages = new Dictionary<InventoryItemEntry, InventoryItemImage>(); 
    }

    public void AddInventoryItemImage(InventoryItemEntry item)
    {
        InventoryItemImage itemImage = imagePool.Get();
        itemImage.gameObject.SetActive(true);
        itemImage.SetPositionAndSize(item.Rect, inventoryCellSize, inventoryCellGap);

        inventoryItemImages.Add(item, itemImage);
    }

    public void RemoveInventoryItemImage(InventoryItemEntry entry)
    {
        bool found = inventoryItemImages.ContainsKey(entry);
        if (found)
        {
            inventoryItemImages[entry].gameObject.SetActive(false);
            imagePool.Release(inventoryItemImages[entry]);
            inventoryItemImages.Remove(entry);
        }
    }

    public void ShowOccupiedItem(InventoryItemEntry entry)
    {
        foreach (InventoryCell cell in inventoryCells)
        {
            cell.Image.color = cell.DefaultColor;
        }

        foreach (Vector2Int position in entry.Rect.allPositionsWithin)
        {
        }
    }

    public void ClearCellColor()
    {
        foreach (InventoryCell cell in inventoryCells)
        {
            cell.Image.color = cell.DefaultColor;
        }
    }

    public void SetCellColor(Vector2Int gridPosition, Color color)
    {
        foreach (InventoryCell cell in inventoryCells)
        {
            cell.Image.color = cell.DefaultColor;
        }

        if(inventoryCellsByPosition.ContainsKey(gridPosition))
        {
            inventoryCellsByPosition[gridPosition].Image.color = color;
        }    
    }

    public void SetCellColor(Vector2Int gridPosition, Vector2Int size, Color color)
    {
        foreach (InventoryCell cell in inventoryCells)
        {
            cell.Image.color = cell.DefaultColor;
        }

        RectInt rect = new RectInt(gridPosition, size);
        foreach (Vector2Int position in rect.allPositionsWithin)
        {
            if (inventoryCellsByPosition.ContainsKey(position))
            {
                inventoryCellsByPosition[position].Image.color = color;
            }
        }
    }
}
