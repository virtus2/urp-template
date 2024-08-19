using UnityEngine;

public enum Animal
{
    Chicken,
    Cow,
    Lion,
    Deer,
    Dragon,
}
[CreateAssetMenu]
public class AnimalData : ScriptableObject
{
    public Core.Character AnimalToSpawn;
    public Animal animal;
    public uint price;
    public Sprite icon;
    public Sprite lockIcon;

    [Header("잠금 - 닭 수")]
    public uint unlockWhenChickenCount = 0;
    [Header("잠금 - 소 수")]
    public uint unlockWhenCowCount = 0;
    [Header("잠금 - 사슴 수")]
    public uint unlockWhenDeerCount = 0;
    [Header("잠금 - 사자 수")]
    public uint unlockWhenLionCount = 0;

    [Header("허기 - 최대 허기 게이지")]
    public float maxHunger = 100f;

    [Header("허기 - 초기 허기 게이지")]
    public float startHunger = 100f;

    [Header("허기 - 허기 알람")]
    public float hungerAlarm = 30f;

    [Header("허기 - 먹이를 먹기 시작하는 허기 게이지")]
    public float hungerToStartEat= 15f;

    [Header("허기 - 초당 허기 게이지 감소량")]
    public float hungerDecreasePerFrame = 0.05f;

    [Header("허기 - 먹이 먹었을 때 충전량")]
    public float hungerEarnedWhenEat= 0.05f;

    [Header("돈 생산 - 먹이 먹고 x이상의 허기됐을때 생산")]
    public float minHungerToSpawnResources = 5.0f;

    [Header("돈 생산 - x초마다 생산")]
    public float spawnResourceTime = 5.0f;

    [Header("돈 생산 - 1회 생성 갯수")]
    public int spawnResourceCount = 1;

    [Header("돈 생산 - 1회 생성 가격")]
    public int resourcePrice = 15;

    [Header("수정X")]
    public Core.BaseCharacterController AnimalController;
}
