using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EggBuyPanel : MonoBehaviour
{
    public uint[] Price;
    private uint index = 0;

    public float delayTime = 2f;

    public Sprite[] Images;
    public Sprite lockImage;


    [SerializeField]
    private Image endImage;

    [SerializeField]
    private TextMeshProUGUI priceText;

    [SerializeField]
    private Image buyImage;

    [SerializeField]
    private Button buyButton;

    private bool unlocked = false;

    public AnimalSpawner spawner;


    private void OnValidate()
    {
        priceText.text = $"${Price[index]}";
    }
    private void Awake()
    {
        buyButton.onClick.AddListener(Buy);
    }

    private void Update()
    {
        if (spawner.animals.ContainsKey(Animal.Lion) && spawner.animals[Animal.Lion].Count > 0)
        {
            unlocked = true;
        }

        if(unlocked)
        {
            buyImage.sprite = Images[index];
        }
        else
        {
            buyImage.sprite = lockImage;
        }
    }

    private void Buy()
    {
        if (PlayerData.balance >= Price[index])
        {
            buyImage.sprite = Images[index];
            PlayerData.balance -= Price[index];
            PlayerData.eggCount++;
            if (PlayerData.eggCount >= Price.Length)
            {
                SceneManager.LoadScene("EndScene");
                return;
            }
            index++;
            priceText.text = $"${Price[index]}";
        }
    }

    private IEnumerator End()
    {
        float timeElapsed = 0f;
        while(timeElapsed <= delayTime)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
