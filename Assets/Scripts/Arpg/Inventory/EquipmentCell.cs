using UnityEngine;
using UnityEngine.UI;

public class EquipmentCell : MonoBehaviour
{
    public EEquipmentType EquipmentType;
    public InventoryItemEntry ItemEntry;
    public Image Image;
    public Image ItemImage;
    public Color DefaultColor;

    private void Awake()
    {
        Image = GetComponent<Image>();
        DefaultColor = Image.color;
    }
}
