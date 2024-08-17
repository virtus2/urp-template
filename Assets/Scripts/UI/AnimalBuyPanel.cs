using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimalBuyPanel : MonoBehaviour
{
    public AnimalData data;

    [SerializeField]
    private Image icon;

    [SerializeField]
    private TextMeshProUGUI priceText;

    [SerializeField]
    private Button buyButton;

    [SerializeField]
    private AnimalSpawner spawner;

    private void OnValidate()
    {
        if(data != null)
        {
            icon.sprite = data.icon;
            priceText.text = $"${data.price}";
        }
    }

    private void Awake()
    {
        buyButton.onClick.AddListener(Buy);
    }

    private void Buy()
    {
        if(PlayerData.balance >= data.price)
        {
            PlayerData.balance -= data.price;

            spawner.Spawn(data);
        }
    }
}
