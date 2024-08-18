using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BalancePanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI balanceText;

    private void Awake()
    {
        PlayerData.OnBalanceChanged += UpdateText;
        UpdateText();
    }

    private void OnDisable()
    {
        PlayerData.OnBalanceChanged -= UpdateText;
    }

    private void UpdateText()
    {
        balanceText.text = $"${PlayerData.balance}";
    }
}
