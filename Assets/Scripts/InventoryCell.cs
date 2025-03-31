using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour
{
    public Vector2Int GridPosition;
    public Image Image;

    private void Awake()
    {
        Image = GetComponent<Image>();
    }
}
