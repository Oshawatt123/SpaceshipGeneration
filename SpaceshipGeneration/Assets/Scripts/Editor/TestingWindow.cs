using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestingWindow : EditorWindow
{
    private static Camera renderCam_FR = null;
    private static Camera renderCam_FL = null;
    private static Camera renderCam_RR = null;
    private static Camera renderCam_RL = null;

    private static TreeGrammarV2 grammarControl = null;
    private static SymbolLookup grammarSymbols = null;
    
    [MenuItem("Window/Test")]
    public static void ShowWindow()
    {
        GetWindow<TestingWindow>("Testing testing");
        
        init();
    }

    private static void init()
    {
        renderCam_FR = GameObject.Find("ShipRenderCam_FR").GetComponent<Camera>();
        renderCam_FL = GameObject.Find("ShipRenderCam_FL").GetComponent<Camera>();
        renderCam_RR = GameObject.Find("ShipRenderCam_RR").GetComponent<Camera>();
        renderCam_RL = GameObject.Find("ShipRenderCam_RL").GetComponent<Camera>();
        
        renderCam_FR.hideFlags = HideFlags.HideAndDontSave;
        renderCam_FR.rect = new Rect(0, 0, 0, 0);
        
        grammarControl = GameObject.FindObjectOfType<TreeGrammarV2>();
        grammarSymbols = GameObject.FindObjectOfType<SymbolLookup>();
    }
    
    void OnGUI()
    {
        GUILayout.Label("What a cool label", EditorStyles.whiteMiniLabel);

        if (renderCam_FR != null)
            Handles.DrawCamera(new Rect(50, 50, 300, 300), renderCam_FR);
        else
            GUILayout.Label("\n\nNo camera available for render!\n\n", EditorStyles.boldLabel);
        
        if (renderCam_FL != null)
            Handles.DrawCamera(new Rect(350, 50, 300, 300), renderCam_FL);
        else
            GUILayout.Label("\n\nNo camera available for render!\n\n", EditorStyles.boldLabel);
        
        if (renderCam_RR != null)
            Handles.DrawCamera(new Rect(50, 350, 300, 300), renderCam_RR);
        else
            GUILayout.Label("\n\nNo camera available for render!\n\n", EditorStyles.boldLabel);
        
        if (renderCam_RL != null)
            Handles.DrawCamera(new Rect(350, 350, 300, 300), renderCam_RL);
        else
            GUILayout.Label("\n\nNo camera available for render!\n\n", EditorStyles.boldLabel);

        
        if (GUILayout.Button("Refresh"))
        {
            init();
        }

        if (GUILayout.Button("Cleanup"))
        {
            grammarControl.Cleanup();
        }

        if (GUILayout.Button("Init ship gen"))
        {
            grammarSymbols.Init();
        }

        if (GUILayout.Button("Generate new ship"))
        {
            grammarControl.BuildShip();
        }
    }
}
