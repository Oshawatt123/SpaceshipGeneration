using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TreeGrammarV2))]
public class TGV2_Inspector : Editor
{
    public override void OnInspectorGUI()
    {
        TreeGrammarV2 root = (TreeGrammarV2) target;
        
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Ship"))
        {
            root.BuildShip();
        }

        if (GUILayout.Button("Cleanup"))
        {
            root.grammarTree.Clean();
        }
    }
}
