using UnityEditor;
using UnityEngine;

public class WLCustomObjectCreator : MonoBehaviour
{
    private const string PATH = "Assets/Watchlist/";
    private const string LOOTTABLE_PATH = "LootTables/NewLootTable.asset";
    private const string LEVELGENINPUT_PATH = "LevelGenInputs/NewLevelGenInput.asset";

    [MenuItem("Custom Objects/Create Loot Table")]
    public static void CreateLootTable()
    {
        SaveAsset(new LootManager(), PATH + LOOTTABLE_PATH);
    }

    [MenuItem("Custom Objects/Create Level Gen Input")]
    public static void CreateLevelGenInupt()
    {
        SaveAsset(new LevelGenInput(), PATH + LEVELGENINPUT_PATH);
    }

    public static void SaveAsset(ScriptableObject asset, string path)
    {
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
