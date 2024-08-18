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

    public void OnTriggerEnter(Collider other)
    {
        if(data.animal == Animal.Lion)
        {
            var animal = other.GetComponent<AnimalCharacter>();
            if (animal)
            {
                if (animal.data.animal == Animal.Chicken)
                {
                    if (IsHungry && currentHunger <= data.hungerToStartEat)
                    {
                        animal.IsDead = true;
                        spawner.Despawn(animal.data.animal, animal.gameObject);
                        currentHunger += data.hungerEarnedWhenEat;
                        if (currentHunger >= data.minHungerToSpawnResources)
                        {
                            meshRenderer.material.color = Color.white;
                            IsHungry = false;
                            IsFull = true;
                        }
                    }
                }
            }
        }
        else
        {
            if (other.transform.CompareTag("Food"))
            {
                if (IsHungry)
                {
                    spawner.DespawnFood(other.gameObject);
                    currentHunger += data.hungerEarnedWhenEat;
                    if (currentHunger >= data.minHungerToSpawnResources)
                    {
                        meshRenderer.material.color = Color.white;
                        IsHungry = false;
                        IsFull = true;
                    }
                }
            }
        }
    }

    protected override void Update()
    {
        base.Update();

        if(!IsFull)
        {
            currentHunger -= Mathf.Clamp(data.hungerDecreasePerFrame * Time.deltaTime, 0, data.maxHunger);
        }
        
        if(currentHunger <= 0)
        {
            IsDead = true;
        }
        if (!IsDead)
        {

            if (IsHungry)
            {
                if (currentHunger >= data.hungerAlarm)
                {
                    IsHungry = false;
                    if (meshRenderer.material.color != Color.white)
                    {
                        meshRenderer.material.color = Color.white;
                    }
                }
                if (currentHunger <= data.hungerToStartEat)
                {
                    if (data.animal == Animal.Lion)
                    {
                        if (spawner.animals[Animal.Chicken].Count > 0)
                        {
                            float minDistance = float.MaxValue;
                            int idx = 0;
                            for (int i = 0; i < spawner.animals[Animal.Chicken].Count; i++)
                            {
                                float distance = (transform.position - spawner.animals[Animal.Chicken][i].transform.position).sqrMagnitude;
                                if (minDistance >= distance)
                                {
                                    idx = i;
                                    minDistance = distance;
                                }
                            }
                            ChaseTarget = spawner.animals[Animal.Chicken][idx];
                            var aiController = Controller as AICharacterController;
                            if (aiController && aiController.aiStateMachine.CurrentState != AIState.Chase)
                            {
                                aiController.aiStateMachine.TransitionToState(AIState.Chase);
                            }
                        }
                    }
                    else
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
                            if (aiController && aiController.aiStateMachine.CurrentState != AIState.Chase)
                            {
                                aiController.aiStateMachine.TransitionToState(AIState.Chase);
                            }
                        }
                    }
                }
            }
            else
            {
                if (currentHunger <= data.hungerAlarm)
                {
                    IsHungry = true;
                    meshRenderer.material.SetColor("_BaseColor", new Color(0.5686275f, 8039216f, 0, 1.0f));
                }
            }

            if (IsFull)
            {
                if (spawnResourcesCount >= data.spawnResourceCount * 3)
                {
                    IsFull = false;
                    spawnResourcesCount = 0;
                }
                if (spawnResourceTimeElapsed >= data.spawnResourceTime)
                {
                    // °ñµå »ý»ê
                    bool isGold = true;
                    if (data.animal == Animal.Lion)
                    {
                        isGold = false;
                    }
                    spawner.SpawnGold(transform.position, isGold);
                    spawnResourceTimeElapsed -= data.spawnResourceTime;
                    spawnResourcesCount++;
                }
                spawnResourceTimeElapsed += Time.deltaTime;
            }
        }

        if (IsDead)
        {
            if(deadTimeElapsed >= 3.0f)
            {
                spawner.Despawn(data.animal, gameObject);
                Destroy(Controller.gameObject);
                Destroy(gameObject);
            }
            
            deadTimeElapsed += Time.deltaTime;
        }
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = IsHungry ? Color.yellow : Color.green;
        if (IsHungry && currentHunger <= data.hungerToStartEat)
        {
            Gizmos.color = Color.red;
        }
        if (IsFull)
        {
            Gizmos.color = Color.cyan;
        }


        Gizmos.DrawSphere(transform.position, currentHunger / data.maxHunger);
    }
}
