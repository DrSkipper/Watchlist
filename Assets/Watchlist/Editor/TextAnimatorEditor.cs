using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TextAnimator))]
public class TextAnimatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Begin"))
        {
            ((TextAnimator)target).Begin();
        }
    }
}
