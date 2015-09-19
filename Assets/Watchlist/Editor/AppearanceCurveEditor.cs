using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AppearanceCurve))]
public class AppearanceCurveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Begin"))
        {
            ((AppearanceCurve)this.target).Begin();
        }
    }
}
