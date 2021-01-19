using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RootPart))]
public class RootPartInspector : Editor
{
    public override void OnInspectorGUI()
    {
        RootPart rootPart = (RootPart) target;

        if (rootPart.NullSymbol)
        {
            rootPart.NullSymbol = GUILayout.Toggle(rootPart.NullSymbol, "Null Symbol");
            return;
        }

        base.OnInspectorGUI();
    }
}
