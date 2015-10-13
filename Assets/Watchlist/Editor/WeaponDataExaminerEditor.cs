using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WeaponDataExaminer))]
public class WeaponDataExaminerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Load"))
        {
            ((WeaponDataExaminer)this.target).Load();
        }

        if (GUILayout.Button("Save"))
        {
            ((WeaponDataExaminer)this.target).Save();
        }
    }
}
