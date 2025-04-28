using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroundItemGenerator : MonoBehaviour
{
    public TreasureClass TreasureClass;

    public Dictionary<string, TreasureClassData> TreasureClassesByName;
    public Dictionary<int, List<TreasureClassData>> TreasureClassesByGroup;

    public string TestDropTreasureClassName = "Act1MagicGroup1";
    public int TestLevel = 18;

    // TODO: 아이템 생성 테스트 함수, 확률 및 통계 표시
    private void Awake()
    {
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

    public void GenerateItemsByTreasureClass(string treasureClassName, int level)
    {
        if (TreasureClassesByName.TryGetValue(treasureClassName, out TreasureClassData data) == false)
            return;

        TreasureClassData treasureClass = data;
        if (TreasureClassesByGroup.TryGetValue(data.Group, out List<TreasureClassData> dataGroup))
        {
            treasureClass = dataGroup.OrderByDescending(x => x.Req_Level <= level).First();
        }

        Queue<string> pickedTreasureClasses = new Queue<string>();
        for (int i = 0; i < treasureClass.Picks; i++)
        {
            string pickResult = treasureClass.Pick();
            pickedTreasureClasses.Enqueue(pickResult);
        }

        while (pickedTreasureClasses.Count >= 1)
        {
            string pickedName = pickedTreasureClasses.Dequeue();

            if (TreasureClassesByName.TryGetValue(pickedName, out TreasureClassData pickedTC))
            {
                for (int i = 0; i < pickedTC.Picks; i++)
                {
                    pickedTreasureClasses.Enqueue(pickedTC.Pick());
                }
            }
            else
            {
                Debug.Log(pickedName);
            }
        }
    }






    [SerializeField] private GroundItemLabel ItemLabelPrefab;
    [SerializeField] private Canvas ItemLabelCanvas;

    [SerializeField] private GroundItem TestGroundItemPrefab;

    [NaughtyAttributes.Button]
    public void TestGenerate()
    {
        GenerateItemsByTreasureClass(TestDropTreasureClassName, TestLevel);

        GroundItem newItem = Instantiate(TestGroundItemPrefab);
        GroundItemLabel newLabel = Instantiate(ItemLabelPrefab, ItemLabelCanvas.transform);

    }
}
