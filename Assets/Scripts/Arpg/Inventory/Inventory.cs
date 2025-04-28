using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;

public class InventoryItemEntry
{
    public EEquipmentType EquipmentType;
    public Vector2Int Size;
    public RectInt Rect;
}

public enum EInventoryControlState
{
    None,
    ItemPickedUp,

}

public interface IItemPickUpHandler
{
    public void PickUpItem(InventoryItemEntry entry);
}

public interface IItemDropHandler
{
    public void DropPickedUpItem();
}

[AddComponentMenu("Arpg/Inventory")]
public class Inventory : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerExitHandler, IItemPickUpHandler, IItemDropHandler
{
    public Action<InventoryItemEntry> OnItemAdded;
    public Action<InventoryItemEntry> OnItemRemoved;

    private ObjectPool<InventoryItemImage> inventoryItemImagePool;

    [Header("Inventory")]
    [SerializeField] [Range(1, 50)] private int height = 5;
    [SerializeField] [Range(1, 50)] private int width = 12;
    [SerializeField] private RectTransform inventoryRectTransform;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private Vector2Int cellSize = new Vector2Int(100, 100);
    [SerializeField] private Vector2Int cellGap = new Vector2Int(5, 5);
    [SerializeField] private InventoryCell inventoryCellPrefab;
    [SerializeField] private InventoryCell[] inventoryCells;
    private InventoryModel inventoryModel;
    private InventoryView inventoryView;
    private InventoryPresenter inventoryPresenter;


    [Header("Equipment")]
    [SerializeField] private EquipmentCell[] equipmentCells;
    private EquipmentModel equipmentModel;
    private EquipmentView equipmentView;
    private EquipmentPresenter equipmentPresenter;


    [Header("Inventory Picked Up Item")]
    [SerializeField] private EInventoryControlState inventoryControlState;
    public Action<InventoryItemEntry> OnItemPickedUp;
    public Action OnItemDropped;


    [Header("Inventory picked up item")]
    public InventoryPickedUpItem PickedUpItemObj;
    public InventoryItemEntry PickedUpItemEntry;


    [Header("Inventory item images")]
    public InventoryItemImage InventoryItemImagePrefab;
    public Transform InventoryItemImageParent;


    private void Awake()
    {
        inventoryItemImagePool = new ObjectPool<InventoryItemImage>(
            createFunc: CreateInventoryItemImage,
            defaultCapacity: height * width,
            maxSize: height * width * 2
        );

        inventoryModel = new InventoryModel(height, width);
        inventoryView = new InventoryView(height, width, cellSize, cellGap, inventoryCells, inventoryItemImagePool);
        inventoryPresenter = new InventoryPresenter(inventoryModel, inventoryView, this, this, inventoryRectTransform, height, width, cellSize, cellGap);

        equipmentModel = new EquipmentModel();
        equipmentView = new EquipmentView(equipmentCells);
        equipmentPresenter = new EquipmentPresenter(equipmentModel, equipmentView, this, this);
    }

    private InventoryItemImage CreateInventoryItemImage()
    {
        InventoryItemImage item = Instantiate(InventoryItemImagePrefab, InventoryItemImageParent);
        return item;
    }

    public void Show()
    {
        if (gameObject.activeInHierarchy)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }

    public void PickUpItem(InventoryItemEntry entry)
    {
        inventoryControlState = EInventoryControlState.ItemPickedUp;
        PickedUpItemEntry = entry;
        PickedUpItemObj.gameObject.SetActive(true);
        // InventoryPickedUpItem.Image = item.
        PickedUpItemObj.SetPositionAndSize(entry.Rect, cellSize, cellGap);

        inventoryPresenter.HandleItemPickedUp(entry);
        equipmentPresenter.HandleItemPickedUp(entry);


        OnItemPickedUp?.Invoke(entry);
    }

    public void DropPickedUpItem()
    {
        inventoryControlState = EInventoryControlState.None;
        PickedUpItemEntry = null;
        PickedUpItemObj.gameObject.SetActive(false);

        inventoryPresenter.HandleItemDropped();
        equipmentPresenter.HandleItemDropped();

        OnItemDropped?.Invoke();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        inventoryPresenter.OnPointerMove(eventData, inventoryControlState);
        equipmentPresenter.OnPointerMove(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        inventoryPresenter.OnPointerDown(eventData, inventoryControlState);
        equipmentPresenter.OnPointerDown(eventData, inventoryControlState);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryPresenter.OnPointerExit(eventData);
        equipmentPresenter.OnPointerExit(eventData);
    }


#if UNITY_EDITOR

    private void OnValidate()
    {
        if(gridLayoutGroup)
        {
            gridLayoutGroup.cellSize = cellSize;
            gridLayoutGroup.spacing = cellGap;
            gridLayoutGroup.constraintCount = width;
        }
    }

    [NaughtyAttributes.Button]
    public void TestAddItem()
    {
        InventoryItemEntry testItemEntry = new InventoryItemEntry();
        testItemEntry.EquipmentType = EEquipmentType.LeftWeapon;
        testItemEntry.Size = new Vector2Int(Random.Range(1, 3), Random.Range(1, 3));
        bool added = inventoryModel.TryAddItem(testItemEntry);
    }

    public void TestRemoveItem()
    {

    }


    [NaughtyAttributes.Button]
    public void InstantiateCells()
    {
        if (inventoryCellPrefab == null)
            return;

        InventoryCell[] cells = transform.GetComponentsInChildren<InventoryCell>();
        foreach (InventoryCell cell in cells)
        {
            DestroyImmediate(cell.gameObject);
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                InventoryCell cell = Instantiate(inventoryCellPrefab, gridLayoutGroup.transform);
                Vector2Int position = new Vector2Int(x, y);
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
            cells[i].GridPosition = new Vector2Int(i / width, i % width);
        }
    }
#endif
}
