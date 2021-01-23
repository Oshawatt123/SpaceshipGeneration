using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShipBase))]
public class ShipBaseInspector : Editor
{
    public override void OnInspectorGUI()
    {
        ShipBase shipBase = (ShipBase) target;
        
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Ship"))
        {
            shipBase.Grow();
        }

        if (GUILayout.Button("FancyShipGen"))
        {
            shipBase.Grow(true);
        }
    }
}
