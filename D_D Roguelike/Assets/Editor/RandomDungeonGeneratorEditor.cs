using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbstractDuneonGenerator), true)]
public class RandomDungeonGeneratorEditor : Editor
{
    AbstractDuneonGenerator generator;

    private void Awake()
    {
        generator = (AbstractDuneonGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Create Dungeon"))
        {
            generator.GenerateDungeon();
        }
    }
}
