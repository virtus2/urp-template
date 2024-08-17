using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AnimalData : ScriptableObject
{
    public Core.Character AnimalToSpawn;
    public uint price;
    public Sprite icon;

    [Header("��� - �ִ� ��� ������")]
    public float maxHunger = 100f;

    [Header("��� - �ʱ� ��� ������")]
    public float startHunger = 100f;

    [Header("��� - ���̸� �Ա� �����ϴ� ��� ������")]
    public float hungerToStartEat= 15f;

    [Header("��� - ������ �� ��� ������ ���ҷ�")]
    public float hungerDecreasePerFrame = 0.05f;

    [Header("��� - ���� �Ծ��� �� ������")]
    public float hungerEarnedWhenEat= 0.05f;

    [Header("�� ���� - 1ȸ ���� ����")]
    public int spawnResourceCount = 1;

    [Header("�� ���� - 1ȸ ���� ����")]
    public int resourcePrice = 15;

    [Header("������ ������ ��Ʈ�ѷ�(�÷��̾ �����Ѵٰų� Ư���� �� ������ ����X")]
    public Core.BaseCharacterController AnimalController;
}
