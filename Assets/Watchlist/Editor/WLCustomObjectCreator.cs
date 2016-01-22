using UnityEditor;
using UnityEngine;

public class WLCustomObjectCreator : MonoBehaviour
{
    private const string PATH = "Assets/Watchlist/";
    private const string LOOTTABLE_PATH = "LootTables/NewLootTable.asset";

    [MenuItem("Custom Objects/Create Loot Table")]
    public static void CreateLootTable()
    {
        SaveAsset(new LootManager(), PATH + LOOTTABLE_PATH);
    }

    public static void SaveAsset(ScriptableObject asset, string path)
    {
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
