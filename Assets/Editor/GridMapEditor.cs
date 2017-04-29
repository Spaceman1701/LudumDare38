using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(GridMap))]
public class GridMapEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridMap map = (GridMap)target;
        if (GUILayout.Button("build map"))
        {
            map.BuildMap();
        }
        if (GUILayout.Button("clear map"))
        {
            map.ClearMap();
        }
    }
}
