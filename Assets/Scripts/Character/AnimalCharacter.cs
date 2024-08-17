using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalCharacter : Core.Character
{
    AnimalData data;
    AnimalSpawner spawner;

    [SerializeField] SkinnedMeshRenderer meshRenderer;
    public float currentHunger;
    public bool IsHungry = false;
    public bool IsFull = false;

    public int spawnResourcesCount = 0;
    public float spawnResourceTimeElapsed = 0f;
    public float deadTimeElapsed = 0f;
    

    protected override void Awake()
    {
        base.Awake();

        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }
    public void SetAnimalData(AnimalData data, AnimalSpawner spawner)
    {
        this.data = data;
        this.spawner = spawner;

        currentHunger = data.startHunger;
        spawnResourceTimeElapsed = data.spawnResourceTime;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Food"))
        {
            Debug.Log("FOOD EAT");
            // TODO: 먹이 먹어서 허기 게이지 채움, 만복상태 돌입체크
            Destroy(collision.gameObject);
            currentHunger += 10.0f;
            if(currentHunger >= 100.0f)
            {
                currentHunger = 100.0f;
                IsFull = true;
            }
        }
    }

    protected override void Update()
    {
        base.Update();

        currentHunger -= Mathf.Clamp(data.hungerDecreasePerFrame * Time.deltaTime, 0, data.maxHunger);
        if(currentHunger <= 0)
        {
            IsDead = true;
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
            if (currentHunger <= data.hungerToStartEat)
            {
                if (spawner.foods.Count > 0)
                {
                    float minDistance = float.MaxValue;
                    int idx = 0;
                    for (int i = 0; i < spawner.foods.Count; i++)
                    {
                        float distance = (transform.position - spawner.foods[i].transform.position).sqrMagnitude;
                        if (minDistance >= distance)
                        {
                            idx = i;
                            minDistance = distance;
                        }
                    }
                    ChaseTarget = spawner.foods[idx];
                    var aiController = Controller as AICharacterController;
                    if (aiController&& aiController.aiStateMachine.CurrentState != AIState.Chase)
                    {
                        aiController.aiStateMachine.TransitionToState(AIState.Chase);
                    }
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
            if(spawnResourcesCount >= data.spawnResourceCount)
            {
                IsFull = false;
            }
            if (spawnResourceTimeElapsed >= data.spawnResourceTime)
            {
                // 골드 생산
                spawnResourceTimeElapsed -= data.spawnResourceTime;
                spawnResourcesCount++;
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
