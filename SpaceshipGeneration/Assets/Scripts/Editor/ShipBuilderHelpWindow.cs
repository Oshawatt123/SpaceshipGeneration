using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShipBuilderHelpWindow : EditorWindow
{
    private void OnGUI()
    {
        GUILayout.Label("To use: ");
        GUILayout.Label("1. Press 'Refresh' to ensure the window has reference to all needed elements");
        GUILayout.Label("2. Press 'Init Ship Gen' to initialize the generator.");
        GUILayout.Label("3. Press 'Generate new ship' to generate a new ship!");
        GUILayout.Label("4. 'Show symmetry blocks' will toggle the material used when applying symmetry");
    }
}
