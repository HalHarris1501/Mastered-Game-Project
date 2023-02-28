using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CustomGrid), true)]
public class CustomAstarSystemEditor : Editor
{
    CustomGrid grid;

    private void Awake()
    {
        grid = (CustomGrid)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Create Grid"))
        {
            grid.MakeGrid();
        }
    }
}
