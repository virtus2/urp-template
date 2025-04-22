using UnityEngine;

public class GroundItemGenerator : MonoBehaviour
{
    [SerializeField] private GroundItemLabel ItemLabelPrefab;
    [SerializeField] private Canvas ItemLabelCanvas;

    [SerializeField] private GroundItem TestGroundItemPrefab;

    [NaughtyAttributes.Button]

    public void TestGenerate()
    {
        GroundItem newItem = Instantiate(TestGroundItemPrefab);
        GroundItemLabel newLabel = Instantiate(ItemLabelPrefab, ItemLabelCanvas.transform);

    }
}
