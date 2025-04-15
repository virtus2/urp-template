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

        // TODO: ��� ���������� �� �κ��丮�Ŵ����� �������� �ٽ� ��� �Ұ���?
        InventoryManager.Instance.ShowPickedUpItem(equippedItem);
       
        EquippedItems.Remove(equipmentType);

        OnItemUnequipped?.Invoke(equipmentType, equippedItem);
    }

    public bool IsEquipped(EEquipmentType equipmentType)
    {
        return EquippedItems.ContainsKey(equipmentType);
    }
}
