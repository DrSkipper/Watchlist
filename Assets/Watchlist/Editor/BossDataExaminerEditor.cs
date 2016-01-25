using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BossDataExaminer))]
public class BossDataExaminerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Load"))
        {
            ((BossDataExaminer)this.target).Load();
        }

        if (GUILayout.Button("Save"))
        {
            ((BossDataExaminer)this.target).Save();
        }
    }
}
