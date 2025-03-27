using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

[RequireComponent(typeof(Inventory))]
public class InventoryUI : MonoBehaviour
{
    public Transform InventoryGrid;
    public InventoryCell InventoryCellPrefab;

    [Header("Any values in GridLayoutGroup will automatically modified by the Inventory.")]
    public GridLayoutGroup GridLayoutGroup;



    private List<InventoryCell> inventoryCells = new List<InventoryCell>();
    private Dictionary<Vector2Int, InventoryCell> inventoryCellsByPosition = new Dictionary<Vector2Int, InventoryCell>(); // [y, x]

    public Vector2Int CellSize = new Vector2Int(100, 100);

    public Vector2Int CellGap = new Vector2Int(5, 5);

    [Header("Inventory item images")]
    public InventoryItemImage InventoryItemImagePrefab;
    public Transform InventoryItemImageParent;

    private ObjectPool<InventoryItemImage> inventoryItemPool;


    private Inventory inventory;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        inventoryItemPool = new ObjectPool<InventoryItemImage>(
            createFunc: CreateInventoryItemImage,
            defaultCapacity: inventory.Height * inventory.Width,
            maxSize: inventory.Height * inventory.Width * 2
        );
    }

    private InventoryItemImage CreateInventoryItemImage()
    {
        InventoryItemImage item = Instantiate(InventoryItemImagePrefab, InventoryItemImageParent);
        return item;
    }

    public void AddInventoryItemImage(InventoryItemEntry item, Action<InventoryItemImage> onPointerDown)
    {
        InventoryItemImage itemImage = inventoryItemPool.Get();
        itemImage.SetPositionAndSize(item.Rect, CellSize, CellGap);
        itemImage.OnPointerDownAction = onPointerDown;
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

        for (int i = 0; i < inventory.Height; i++)
        {
            for (int j = 0; j < inventory.Width; j++)
            {
                InventoryCell cell = Instantiate(InventoryCellPrefab, InventoryGrid.transform);
                Vector2Int position = new Vector2Int(i, j);
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
