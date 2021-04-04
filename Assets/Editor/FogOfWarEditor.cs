using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FogOfWar))]
public class FogOfWarEditor : Editor
{
    public override void OnInspectorGUI()
    {
        FogOfWar fogOfWar = (FogOfWar)target;
        if (GUILayout.Button("Toggle Fog Of War"))
        {
            fogOfWar.IsEnabled = !fogOfWar.IsEnabled;
        }
    }
}
