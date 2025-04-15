using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

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

public class EquipmentView : MonoBehaviour
{
    public List<EquipmentCell> EquipmentCells;

    private Dictionary<EEquipmentType, EquipmentCell> equipmentCellsByType;

    private void Awake()
    {
        equipmentCellsByType = new Dictionary<EEquipmentType, EquipmentCell>();

        foreach (EquipmentCell cell in EquipmentCells)
        {
            equipmentCellsByType.Add(cell.EquipmentType, cell);
        }
    }

    public void HighlightEquipmentCell(InventoryItemEntry entry)
    {
        if (equipmentCellsByType[entry.EquipmentType].ItemEntry == null)
        {
            // TODO: ���ʿ� ���� ������ �� ó��
            // ex: ����, ����
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
