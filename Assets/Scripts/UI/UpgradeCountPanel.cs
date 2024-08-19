using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeCountPanel : MonoBehaviour
{
    public uint Price = 200;

    public Sprite[] Images;


    [SerializeField]
    private TextMeshProUGUI priceText;

    [SerializeField]
    private Button buyButton;

    [SerializeField]
    private Image buyImage;

    private void Awake()
    {
        buyButton.onClick.AddListener(Buy);
    }

    private void Buy()
    {
        if (PlayerData.balance >= Price)
        {
            PlayerData.balance -= Price;
            PlayerData.foodCountUpgrade++;
            if (PlayerData.foodCountUpgrade >= PlayerData.foodCountMaxUpgrade)
            {
                priceText.text = "Complete";
                return;
            }
            Price += 100 * PlayerData.foodCountUpgrade;
            priceText.text = $"${Price}";
            buyImage.sprite = Images[PlayerData.foodCountUpgrade];
        }
    }
}
