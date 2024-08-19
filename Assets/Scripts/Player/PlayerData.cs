using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public GameData gameData;

    private static uint _balance = 250;
    public static uint balance
    {
        get
        {
            return _balance;
        }
        set
        {
            _balance = value;
            OnBalanceChanged?.Invoke();
        }
    }

    public static uint chickenCount = 0;
    public static uint cowCount = 0;
    public static uint lionCount = 0;
    public static uint eggCount = 0;

    public static uint foodMaxUpgrade = 5;
    public static uint foodCountMaxUpgrade = 4;
    public static uint foodUpgrade = 0;
    public static uint foodCountUpgrade = 0;

    public static Action OnBalanceChanged;
}
