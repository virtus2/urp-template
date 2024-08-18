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

    public bool unlocked = false;

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

        if (data.unlockWhenLionCount == 0 && data.unlockWhenCowCount == 0 && data.unlockWhenChickenCount == 0)
            unlocked = true;
    }

    private void Update()
    {
        if(data == null)
        {
            return;
        }

        bool chickenCondition = false;
        if(data.unlockWhenChickenCount > 0)
        {
            if(spawner.animals.ContainsKey(Animal.Chicken))
            {
                chickenCondition |= spawner.animals[Animal.Chicken].Count >= data.unlockWhenChickenCount;
            }
        }
        else
        {
            chickenCondition = true;
        }
        bool cowCondition = false;
        if (data.unlockWhenCowCount > 0)
        {
            if (spawner.animals.ContainsKey(Animal.Cow))
            {
                cowCondition |= spawner.animals[Animal.Cow].Count >= data.unlockWhenCowCount;
            }
        }
        else
        {
            cowCondition = true;
        }

        if (chickenCondition && cowCondition)
            unlocked = true;


        if(unlocked)
        {
            icon.sprite = data.icon;
        }
        else
        {
            icon.sprite = data.lockIcon;
        }
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
