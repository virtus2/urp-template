using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GroundItemLabel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI TextMesh;
    [SerializeField] private Image BackgroundImage;

    public Action<GroundItem> OnLabelClicked;
    public GroundItem GroundItem;

    public void SetText(string text)
    {
        TextMesh.text = text;
    }

    public void SetTextColor(Color color)
    {
        TextMesh.color = color;
    }

    public void SetBackgroundImageColor(Color color)
    {
        BackgroundImage.color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnLabelClicked?.Invoke(GroundItem);
    }
}
