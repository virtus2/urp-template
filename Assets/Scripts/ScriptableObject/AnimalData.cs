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

    [Header("��� - �� ��")]
    public uint unlockWhenChickenCount = 0;
    [Header("��� - �� ��")]
    public uint unlockWhenCowCount = 0;
    [Header("��� - �罿 ��")]
    public uint unlockWhenDeerCount = 0;
    [Header("��� - ���� ��")]
    public uint unlockWhenLionCount = 0;

    [Header("��� - �ִ� ��� ������")]
    public float maxHunger = 100f;

    [Header("��� - �ʱ� ��� ������")]
    public float startHunger = 100f;

    [Header("��� - ��� �˶�")]
    public float hungerAlarm = 30f;

    [Header("��� - ���̸� �Ա� �����ϴ� ��� ������")]
    public float hungerToStartEat= 15f;

    [Header("��� - �ʴ� ��� ������ ���ҷ�")]
    public float hungerDecreasePerFrame = 0.05f;

    [Header("��� - ���� �Ծ��� �� ������")]
    public float hungerEarnedWhenEat= 0.05f;

    [Header("�� ���� - ���� �԰� x�̻��� �������� ����")]
    public float minHungerToSpawnResources = 5.0f;

    [Header("�� ���� - x�ʸ��� ����")]
    public float spawnResourceTime = 5.0f;

    [Header("�� ���� - 1ȸ ���� ����")]
    public int spawnResourceCount = 1;

    [Header("�� ���� - 1ȸ ���� ����")]
    public int resourcePrice = 15;

    [Header("����X")]
    public Core.BaseCharacterController AnimalController;
}
