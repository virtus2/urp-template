using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerData
{
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
    public static float foodHunger = 60f;
    public static uint foodPrice = 5;
    public static uint foodMaxCount = 3;
    public static uint goldAmount = 25;
    public static uint diamondAmount = 100;

    public static uint chickenCount = 0;
    public static uint cowCount = 0;
    public static uint lionCount = 0;

    public static Action OnBalanceChanged;
}
