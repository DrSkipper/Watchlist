﻿using UnityEngine;
using UnityEditor;

//http://stackoverflow.com/questions/29819697/manually-edit-unity3d-collider-coordinates
[CustomEditor(typeof(PolygonCollider2D))]
public class PolygonCollider2DEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var collider = (PolygonCollider2D)target;
        var points = collider.points;
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = EditorGUILayout.Vector2Field(i.ToString(), points[i]);
        }
        collider.points = points;
        EditorUtility.SetDirty(target);
    }
}
