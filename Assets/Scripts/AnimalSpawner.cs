using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject Food;

    [SerializeField]
    private GameObject SpawnCube;

    public List<GameObject> foods = new List<GameObject>(1000);

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
    }

    public void SpawnFood(Vector3 position)
    {
        var food = Instantiate(Food, position, Random.rotation);
        foods.Add(food);
    }
}
