using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EquipmentView), typeof(Equipment))]
public class EquipmentPresenter : MonoBehaviour, IPointerMoveHandler, IPointerDownHandler, IPointerExitHandler
{
    private Equipment equipment;
    private EquipmentView equipmentView;

    private void Awake()
    {
        equipment = GetComponent<Equipment>();
        equipmentView = GetComponent<EquipmentView>();
    }

    private void OnEnable()
    {
        InventoryManager.OnItemPickedUp += HandleItemPickedUp;
        InventoryManager.OnItemDropped += HandleItemDropped;

        equipment.OnItemEquipped += HandleItemEquipped;
        equipment.OnItemUnequipped += HandleItemUnequipped;
    }

    private void OnDisable()
    {
        InventoryManager.OnItemPickedUp -= HandleItemPickedUp;
        InventoryManager.OnItemDropped -= HandleItemDropped;

        equipment.OnItemEquipped -= HandleItemEquipped;
        equipment.OnItemUnequipped -= HandleItemUnequipped;
    }

    private void HandleItemPickedUp(InventoryItemEntry entry)
    {
        equipmentView.HighlightEquipmentCell(entry);   
    }

    private void HandleItemDropped()
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

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject == null)
            return;

        EquipmentCell cell = eventData.pointerCurrentRaycast.gameObject.GetComponent<EquipmentCell>();
        if (cell)
        {
            switch (InventoryManager.Instance.InventoryControlState)
            {
                case EInventoryControlState.None:
                    {

                    }
                    break;
                case EInventoryControlState.ItemPickedUp:
                    {
                        if (cell.EquipmentType == InventoryManager.Instance.PickedUpItemEntry.EquipmentType)
                        {
                            equipment.Equip(InventoryManager.Instance.PickedUpItemEntry);
                        }
                    }
                    break;

            }
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
