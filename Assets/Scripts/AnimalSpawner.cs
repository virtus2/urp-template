using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject Gold;

    [SerializeField]
    private GameObject Diamond;

    [SerializeField]
    private GameObject Food;

    [SerializeField]
    private GameObject SpawnCube;

    public List<GameObject> foods = new List<GameObject>(1000);

    public Dictionary<Animal, List<GameObject>> animals = new Dictionary<Animal, List<GameObject>>();

    private Bounds SpawnBounds;

    private void Awake()
    {
        SpawnBounds = new Bounds(SpawnCube.transform.position, SpawnCube.transform.localScale);
    }

    public void Spawn(AnimalData data)
    {
        var target = new Vector3(
            Random.Range(SpawnBounds.min.x, SpawnBounds.max.x),
            Random.Range(SpawnBounds.min.y, SpawnBounds.max.y),
            Random.Range(SpawnBounds.min.z, SpawnBounds.max.z)
        );

        var controller = Instantiate(data.AnimalController);
        var character = Instantiate(data.AnimalToSpawn, target, Quaternion.identity) as AnimalCharacter;
        controller.SetCharacter(character);
        character.SetController(controller);
        character.SetAnimalData(data, this);

        if (!animals.ContainsKey(data.animal))
        {
            animals.Add(data.animal, new List<GameObject>());
        }
        animals[data.animal].Add(character.gameObject);
    }

    public void Despawn(Animal animal, GameObject go)
    {
        animals[animal].Remove(go);
    }

    public void SpawnFood(Vector3 position)
    {
        if (PlayerData.foodMaxCount > foods.Count)
        {
            if(PlayerData.balance >= PlayerData.foodPrice)
            {
                PlayerData.balance -= PlayerData.foodPrice;
                var food = Instantiate(Food, position, Random.rotation);
                foods.Add(food);
            }
        }
    }
    
    public void DespawnFood(GameObject go)
    {
        foods.Remove(go);
        Destroy(go);
    }

    public void SpawnGold(Vector3 position, bool isGold)
    {
        if (isGold)
        {
            var gold = Instantiate(Gold, position, Random.rotation);
        }
        else
        {
            var diamond = Instantiate(Diamond, position, Random.rotation);
        }
    }
}
