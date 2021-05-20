using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SymbolLookup))]
public class SymblLkup_Inspector : Editor
{
    public override void OnInspectorGUI()
    {
        SymbolLookup root = (SymbolLookup) target;
        
        base.OnInspectorGUI();

        if (GUILayout.Button("Init"))
        {
            root.Init();
        }
    }
}