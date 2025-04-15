using System;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public EInventoryControlState InventoryControlState = EInventoryControlState.None;

    public static Action<InventoryItemEntry> OnItemPickedUp;
    public static Action OnItemDropped;

    [Header("Inventory picked up item")]
    public InventoryPickedUpItem PickedUpItemObj;
    public InventoryItemEntry PickedUpItemEntry;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        Instance = this;
    }

    public void HidePickedUpItem()
    {
    }
    
    public void PickUpItem(InventoryItemEntry entry, Vector2Int cellSize, Vector2Int cellGap)
    {
        InventoryControlState = EInventoryControlState.ItemPickedUp;
        PickedUpItemEntry = entry;
        PickedUpItemObj.gameObject.SetActive(true);
        // InventoryPickedUpItem.Image = item.
        PickedUpItemObj.SetPositionAndSize(entry.Rect, cellSize, cellGap);

        OnItemPickedUp?.Invoke(entry);
    }

    public void ShowPickedUpItem(InventoryItemEntry item)
    {

    }

    public void DropPickedUpItem()
    {
        InventoryControlState = EInventoryControlState.None;
        PickedUpItemEntry = null;
        PickedUpItemObj.gameObject.SetActive(false);

        OnItemDropped?.Invoke();
    }
}
