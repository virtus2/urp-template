using System.Collections.Generic;
using UnityEngine;

public enum EEquipmentType
{
    LeftWeapon,
    RightWeapon,
    Helmet,
    Chest,
    Amulet,
    Gloves,
    LeftRing,
    RightRing,
    Belt,
    Boots,
}

public class EquipmentView
{
    public List<EquipmentCell> EquipmentCells;

    private Dictionary<EEquipmentType, EquipmentCell> equipmentCellsByType;

    public EquipmentView(EquipmentCell[] cells)
    {
        equipmentCellsByType = new Dictionary<EEquipmentType, EquipmentCell>();

        foreach (EquipmentCell cell in cells)
        {
            equipmentCellsByType.Add(cell.EquipmentType, cell);
        }
    }

    public void HighlightEquipmentCell(InventoryItemEntry entry)
    {
        if (equipmentCellsByType[entry.EquipmentType].ItemEntry == null)
        {
            // TODO: 양쪽에 장착 가능한 것 처리
            // ex: 반지, 무기
            if (entry.EquipmentType == EEquipmentType.LeftWeapon)
            {

            }

            equipmentCellsByType[entry.EquipmentType].Image.color = Color.green;
        }
    }

    public void ResetHighlight()
    {
        foreach (EquipmentCell cell in equipmentCellsByType.Values)
        {
            cell.Image.color = cell.DefaultColor;
        }
    }

    public void SetEquipmentImage(EEquipmentType equipmentType, InventoryItemEntry entry)
    {
        equipmentCellsByType[equipmentType].ItemImage.gameObject.SetActive(true);
        //
    }

    public void HideEquipmentImage(EEquipmentType equipmentType)
    {
        equipmentCellsByType[equipmentType].ItemImage.gameObject.SetActive(false);
    }
}
