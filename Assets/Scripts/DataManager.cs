using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public ItemClassCategory ItemClassCategory;
    public ItemClass ItemClass;
    public WeaponBaseData WeaponBaseData;
    public TreasureClass TreasureClass;

    public Dictionary<string, TreasureClassData> TreasureClassesByName;
    public Dictionary<int, List<TreasureClassData>> TreasureClassesByGroup;


    private void Awake()
    {
        InitTreasureClass();
    }

    private void InitTreasureClass()
    {
        Debug.Log(StringUtility.ToColoredString("Initialize TreasureClass: Complete", Color.green));
        // TODO: 아이템 생성 테스트 함수, 확률 및 통계 표시
        TreasureClassesByName = new Dictionary<string, TreasureClassData>();
        TreasureClassesByGroup = new Dictionary<int, List<TreasureClassData>>();

        foreach (TreasureClassData data in TreasureClass.dataArray)
        {
            TreasureClassesByName.Add(data.Class_Name, data);

            if (TreasureClassesByGroup.ContainsKey(data.Group))
                TreasureClassesByGroup[data.Group].Add(data);
            else
            {
                List<TreasureClassData> dataGroup = new List<TreasureClassData>();
                TreasureClassesByGroup.Add(data.Group, dataGroup);
                dataGroup.Add(data);
            }
        }
    }


}
