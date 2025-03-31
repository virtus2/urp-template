using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

[RequireComponent(typeof(Inventory))]
public class InventoryView : MonoBehaviour
{
    private Inventory inventory;

    [Header("Inventory grid")]
    public Transform InventoryGrid;

    [Header("Any values in GridLayoutGroup will automatically modified by the Inventory.")]
    public GridLayoutGroup GridLayoutGroup;

    [Header("Inventory cell prefab")]
    public InventoryCell InventoryCellPrefab;

    [Header("Inventory cell size & gap")]
    public Vector2Int CellSize = new Vector2Int(100, 100);
    public Vector2Int CellGap = new Vector2Int(5, 5);

    private List<InventoryCell> inventoryCells = new List<InventoryCell>();
    private Dictionary<Vector2Int, InventoryCell> inventoryCellsByPosition = new Dictionary<Vector2Int, InventoryCell>();


    [Header("Inventory item images")]
    public InventoryItemImage InventoryItemImagePrefab;
    public Transform InventoryItemImageParent;

    private Dictionary<InventoryItemEntry, InventoryItemImage> inventoryItemImages = new Dictionary<InventoryItemEntry, InventoryItemImage>();
    private ObjectPool<InventoryItemImage> inventoryItemImagePool;

    [Header("Inventory picked up item")]
    public InventoryPickedUpItem InventoryPickedUpItem;


    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        inventoryItemImagePool = new ObjectPool<InventoryItemImage>(
            createFunc: CreateInventoryItemImage,
            defaultCapacity: inventory.Height * inventory.Width,
            maxSize: inventory.Height * inventory.Width * 2
        );

        InventoryCell[] cells = transform.GetComponentsInChildren<InventoryCell>();
        for (int i = 0; i < cells.Length; i++)
        {
            Vector2Int gridPosition = new Vector2Int(i % inventory.Width, i / inventory.Width);
            cells[i].GridPosition = gridPosition;
            inventoryCells.Add(cells[i]);
            inventoryCellsByPosition.Add(gridPosition, cells[i]);
        }
    }

    private InventoryItemImage CreateInventoryItemImage()
    {
        InventoryItemImage item = Instantiate(InventoryItemImagePrefab, InventoryItemImageParent);
        return item;
    }

    public void AddInventoryItemImage(InventoryItemEntry item)
    {
        InventoryItemImage itemImage = inventoryItemImagePool.Get();
        itemImage.gameObject.SetActive(true);
        itemImage.SetPositionAndSize(item.Rect, CellSize, CellGap);

        inventoryItemImages.Add(item, itemImage);
    }

    public void RemoveInventoryItemImage(InventoryItemEntry entry)
    {
        bool found = inventoryItemImages.ContainsKey(entry);
        if (found)
        {
            inventoryItemImages[entry].gameObject.SetActive(false);
            inventoryItemImagePool.Release(inventoryItemImages[entry]);
            inventoryItemImages.Remove(entry);
        }
    }

    public void ShowPickedUpItem(InventoryItemEntry item)
    {
        InventoryPickedUpItem.gameObject.SetActive(true);
        // InventoryPickedUpItem.Image = item.
        InventoryPickedUpItem.SetPositionAndSize(item.Rect, CellSize, CellGap);
    }

    public void HidePickedUpItem()
    {
        InventoryPickedUpItem.gameObject.SetActive(false);
    }

    public void SetPickedUpItemPosition(Vector2 position)
    {
        InventoryPickedUpItem.transform.position = position;
    }

    public void ShowOccupiedItem(InventoryItemEntry entry)
    {
        foreach (InventoryCell cell in inventoryCells)
        {
            cell.Image.color = Color.white;
        }

        foreach (Vector2Int position in entry.Rect.allPositionsWithin)
        {
        }
    }

    public void ClearCellColor()
    {
        foreach (InventoryCell cell in inventoryCells)
        {
            cell.Image.color = Color.white;
        }
    }

    public void SetCellColor(Vector2Int gridPosition, Color color)
    {
        foreach (InventoryCell cell in inventoryCells)
        {
            cell.Image.color = Color.white;
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
            cell.Image.color = Color.white;
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

#if UNITY_EDITOR
    private void OnValidate()
    {
        inventory = GetComponent<Inventory>();
        GridLayoutGroup.cellSize = CellSize;
        GridLayoutGroup.spacing = CellGap;
        GridLayoutGroup.constraintCount = inventory.Width;
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
        inventoryCells.Clear();
        inventoryCellsByPosition.Clear();

        for (int y = 0; y < inventory.Height; y++)
        {
            for (int x = 0; x < inventory.Width; x++)
            {
                InventoryCell cell = Instantiate(InventoryCellPrefab, InventoryGrid.transform);
                Vector2Int position = new Vector2Int(x, y);
                inventoryCells.Add(cell);
                inventoryCellsByPosition.Add(position, cell);
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
            cells[i].GridPosition = new Vector2Int(i / inventory.Width, i % inventory.Width);
        }
    }
#endif
}
