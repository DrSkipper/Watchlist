using UnityEngine;
using UnityEditor;

public static class EditorExtensions
{
    [MenuItem("Edit/Reset Playerprefs")]
    public static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Player prefs wiped");
    }
}
