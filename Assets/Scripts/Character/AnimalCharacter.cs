using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalCharacter : Core.Character
{
    AnimalData data;

    [SerializeField] SkinnedMeshRenderer meshRenderer;
    public float currentHunger;
    public bool IsHungry = false;
    public bool IsFull = false;

    public float spawnResourceTimeElapsed = 0f;
    public float deadTimeElapsed = 0f;
    

    protected override void Awake()
    {
        base.Awake();

        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }
    public void SetAnimalData(AnimalData data)
    {
        this.data = data;

        currentHunger = data.startHunger;
        spawnResourceTimeElapsed = data.spawnResourceTime;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Food"))
        {
            // TODO: 먹이 먹어서 허기 게이지 채움, 만복상태 돌입체크
        }
    }

    protected override void Update()
    {
        base.Update();

        currentHunger -= Mathf.Clamp(data.hungerDecreasePerFrame * Time.deltaTime, 0, data.maxHunger);
        if(currentHunger <= 0)
        {
            IsDead = true;
            return;
        }
        if(IsHungry)
        {
            if(currentHunger >= data.hungerAlarm)
            {
                IsHungry = false;
                if (meshRenderer.material.color != Color.white)
                {
                    meshRenderer.material.color = Color.white;
                }
            }
        }
        else
        {
            if (currentHunger <= data.hungerAlarm)
            {
                IsHungry = true;
                meshRenderer.material.color = new Color(145, 205, 0, 255);
            }
        }

        if(IsFull)
        {
            if (spawnResourceTimeElapsed >= data.spawnResourceTime)
            {
                // 골드 생산
                spawnResourceTimeElapsed -= data.spawnResourceTime;
            }
            spawnResourceTimeElapsed += Time.deltaTime;
        }

        if (IsDead)
        {
            if(deadTimeElapsed >= 3.0f)
            {
                Destroy(Controller);
                Destroy(gameObject);
            }
            deadTimeElapsed += Time.deltaTime;
        }
    }
}
