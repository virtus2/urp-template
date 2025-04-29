using Core.Player;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GroundItemGenerator : MonoBehaviour
{
    public string TestDropTreasureClassName = "Act1MagicGroup1";
    public int TestLevel = 18;


    public void GenerateItemsByTreasureClass(string treasureClassName, int level)
    {
        if (DataManager.Instance.TreasureClassesByName.TryGetValue(treasureClassName, out TreasureClassData data) == false)
            return;

        TreasureClassData treasureClass = data;
        if (DataManager.Instance.TreasureClassesByGroup.TryGetValue(data.Group, out List<TreasureClassData> dataGroup))
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

            if (DataManager.Instance.TreasureClassesByName.TryGetValue(pickedName, out TreasureClassData pickedTC))
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
        newLabel.GroundItem = newItem;
        newLabel.OnLabelClicked += PlayerInstance.Instance.Inventory.PickUpItemFromGround;

    }
}
