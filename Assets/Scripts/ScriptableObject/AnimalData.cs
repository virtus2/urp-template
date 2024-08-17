using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AnimalData : ScriptableObject
{
    public Core.Character AnimalToSpawn;
    public uint price;
    public Sprite icon;

    [Header("허기 - 최대 허기 게이지")]
    public float maxHunger = 100f;

    [Header("허기 - 초기 허기 게이지")]
    public float startHunger = 100f;

    [Header("허기 - 먹이를 먹기 시작하는 허기 게이지")]
    public float hungerToStartEat= 15f;

    [Header("허기 - 프레임 당 허기 게이지 감소량")]
    public float hungerDecreasePerFrame = 0.05f;

    [Header("허기 - 먹이 먹었을 때 충전량")]
    public float hungerEarnedWhenEat= 0.05f;

    [Header("돈 생산 - 1회 생성 갯수")]
    public int spawnResourceCount = 1;

    [Header("돈 생산 - 1회 생성 가격")]
    public int resourcePrice = 15;

    [Header("동물을 제어할 컨트롤러(플레이어가 조종한다거나 특별한 일 없으면 수정X")]
    public Core.BaseCharacterController AnimalController;
}
