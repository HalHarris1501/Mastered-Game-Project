using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomMaker), true)]
public class RoomMakerEditor : Editor
{
    RoomMaker maker;

    private void Awake()
    {
        maker = (RoomMaker)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Convert to Room"))
        {
            maker.MakeRoom();
        }
    }
}
