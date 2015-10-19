using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyDataExaminer))]
public class EnemyDataExaminerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Load"))
        {
            ((EnemyDataExaminer)this.target).Load();
        }

        if (GUILayout.Button("Save"))
        {
            ((EnemyDataExaminer)this.target).Save();
        }
    }
}
