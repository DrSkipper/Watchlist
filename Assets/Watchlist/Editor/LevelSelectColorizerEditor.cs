using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelSelectColorizer))]
public class LevelSelectColorizerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUI.changed)
        {
            ((LevelSelectColorizer)target).UpdateColor();
        }
    }
}
