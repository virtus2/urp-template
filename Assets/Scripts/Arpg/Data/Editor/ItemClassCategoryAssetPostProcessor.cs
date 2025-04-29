using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class ItemClassCategoryAssetPostprocessor : AssetPostprocessor 
{
    private static readonly string filePath = "Assets/ScriptableObjects/QuickSheet/실험 250422.xlsx";
    private static readonly string assetFilePath = "Assets/ScriptableObjects/QuickSheet/ItemClassCategory.asset";
    private static readonly string sheetName = "ItemClassCategory";
    
    static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string asset in importedAssets) 
        {
            if (!filePath.Equals (asset))
                continue;
                
            ItemClassCategory data = (ItemClassCategory)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ItemClassCategory));
            if (data == null) {
                data = ScriptableObject.CreateInstance<ItemClassCategory> ();
                data.SheetName = filePath;
                data.WorksheetName = sheetName;
                AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
                //data.hideFlags = HideFlags.NotEditable;
            }
            
            //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<ItemClassCategoryData>().ToArray();		

            //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            //EditorUtility.SetDirty (obj);

            ExcelQuery query = new ExcelQuery(filePath, sheetName);
            if (query != null && query.IsValid())
            {
                data.dataArray = query.Deserialize<ItemClassCategoryData>().ToArray();
                ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
                EditorUtility.SetDirty (obj);
            }
        }
    }
}
