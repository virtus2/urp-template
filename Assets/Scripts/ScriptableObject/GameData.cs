
using UnityEngine;

[CreateAssetMenu]
public class GameData : ScriptableObject
{
    public float foodHunger = 60f;
    public uint foodPrice = 5;
    public uint foodMaxCount = 3;
    public uint goldAmount = 25;
    public uint diamondAmount = 100;
}