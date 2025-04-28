using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using static UnityEngine.EventSystems.EventTrigger;

public class EquipmentPresenter
{
    private EquipmentModel equipmentModel;
    private EquipmentView equipmentView;
    private IItemPickUpHandler itemPickUpHandler;
    private IItemDropHandler itemDropHandler;

    private InventoryItemEntry currentPickedUpItem;

    public EquipmentPresenter(EquipmentModel model, EquipmentView view, IItemPickUpHandler pickUpHandler, IItemDropHandler dropHandler)
    {
        equipmentModel = model;
        equipmentView = view;

        itemPickUpHandler = pickUpHandler;
        itemDropHandler = dropHandler;

        equipmentModel.OnItemEquipped += HandleItemEquipped;
        equipmentModel.OnItemUnequipped += HandleItemUnequipped;
    }

    public void HandleItemPickedUp(InventoryItemEntry entry)
    {
        equipmentView.HighlightEquipmentCell(entry);
        currentPickedUpItem = entry;
    }

    public void HandleItemDropped()
    {
        equipmentView.ResetHighlight();
    }

    private void HandleItemEquipped(EEquipmentType equipmentType, InventoryItemEntry entry)
    {
        equipmentView.SetEquipmentImage(equipmentType, entry);
    }

    private void HandleItemUnequipped(EEquipmentType equipmentType, InventoryItemEntry entry)
    {
        equipmentView.HideEquipmentImage(equipmentType);
    }

    public void OnPointerDown(PointerEventData eventData, EInventoryControlState state)
    {
        if (eventData.pointerCurrentRaycast.gameObject == null)
            return;

        EquipmentCell cell = eventData.pointerCurrentRaycast.gameObject.GetComponent<EquipmentCell>();
        if (cell == null)
            return;

        InventoryItemEntry equippedItem = equipmentModel.GetEquippedItem(cell.EquipmentType);
        switch (state)
        {
            case EInventoryControlState.None:
                if (equippedItem != null)
                {
                    if (itemPickUpHandler != null)
                    {
                        itemPickUpHandler.PickUpItem(equippedItem);
                        equipmentModel.Unequip(cell.EquipmentType);
                    }
                }

                break;
            case EInventoryControlState.ItemPickedUp:
                if (cell.EquipmentType != currentPickedUpItem.EquipmentType || equippedItem != null)
                {
                    // ÀåÂø ºÒ°¡

                }
                else
                {
                    equipmentModel.Equip(currentPickedUpItem);
                    itemDropHandler?.DropPickedUpItem();
                }

                break;
        }
    }
    

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log(eventData);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        Debug.Log(eventData);
    }
}
