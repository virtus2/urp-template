using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalCharacter : Core.Character
{
    AnimalData data;

    public float currentHunger;

    public void SetAnimalData(AnimalData data)
    {
        this.data = data;

        currentHunger = data.startHunger;
    }

    protected override void Update()
    {
        base.Update();

        currentHunger -= data.hungerDecreasePerFrame * Time.deltaTime;
    }
}
