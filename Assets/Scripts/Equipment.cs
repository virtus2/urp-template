using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Equipment : MonoBehaviour
{
    public Action<EEquipmentType, InventoryItemEntry> OnItemEquipped;
    public Action<EEquipmentType, InventoryItemEntry> OnItemUnequipped;

    private Dictionary<EEquipmentType, InventoryItemEntry> EquippedItems = new Dictionary<EEquipmentType, InventoryItemEntry>();

    public void Equip(InventoryItemEntry entry)
    {
        InventoryManager.Instance.DropPickedUpItem();
        EquippedItems.Add(entry.EquipmentType, entry);

        OnItemEquipped?.Invoke(entry.EquipmentType, entry);
    }
    public void Unequip(EEquipmentType equipmentType)
    {
        
        InventoryItemEntry equippedItem = EquippedItems[equipmentType];

        // TODO: 장비 장착해제할 때 인벤토리매니저로 아이템을 다시 들게 할건지?
        InventoryManager.Instance.ShowPickedUpItem(equippedItem);
       
        EquippedItems.Remove(equipmentType);

        OnItemUnequipped?.Invoke(equipmentType, equippedItem);
    }

    public bool IsEquipped(EEquipmentType equipmentType)
    {
        return EquippedItems.ContainsKey(equipmentType);
    }
}
