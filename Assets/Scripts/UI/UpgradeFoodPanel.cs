using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeFoodPanel : MonoBehaviour
{
    public uint Price = 200;

    [SerializeField]
    private TextMeshProUGUI priceText;

    [SerializeField]
    private Button buyButton;

    private void OnValidate()
    {
        priceText.text = $"${Price}";
    }
    private void Awake()
    {
        buyButton.onClick.AddListener(Buy);
    }

    private void Update()
    {
    }

    private void Buy()
    {
        if (PlayerData.balance >= Price && PlayerData.foodUpgrade < PlayerData.foodMaxUpgrade)
        {
            PlayerData.balance -= Price;
            PlayerData.foodUpgrade++;
            if (PlayerData.foodUpgrade >= PlayerData.foodMaxUpgrade)
            {
                priceText.text = "Complete";
                return;
            }
            Price += 200 * (PlayerData.foodUpgrade+1);
            priceText.text = $"${Price}";
        }
    }
}
