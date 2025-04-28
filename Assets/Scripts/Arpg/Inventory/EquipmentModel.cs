using System.Collections.Generic;
using System;
using UnityEngine;

public class EquipmentModel
{
    public Action<EEquipmentType, InventoryItemEntry> OnItemEquipped;
    public Action<EEquipmentType, InventoryItemEntry> OnItemUnequipped;

    private Dictionary<EEquipmentType, InventoryItemEntry> EquippedItems = new Dictionary<EEquipmentType, InventoryItemEntry>();

    public void Equip(InventoryItemEntry entry)
    {
        EquippedItems.Add(entry.EquipmentType, entry);

        OnItemEquipped?.Invoke(entry.EquipmentType, entry);
    }

    public void Unequip(EEquipmentType equipmentType)
    {
        InventoryItemEntry equippedItem = EquippedItems[equipmentType];
        EquippedItems.Remove(equipmentType);

        OnItemUnequipped?.Invoke(equipmentType, equippedItem);
    }

    public bool IsEquipped(EEquipmentType equipmentType)
    {
        return EquippedItems.ContainsKey(equipmentType);
    }

    public InventoryItemEntry GetEquippedItem(EEquipmentType equipmentType)
    {
        if (EquippedItems.ContainsKey(equipmentType))
            return EquippedItems[equipmentType];

        return null;
    }
}
