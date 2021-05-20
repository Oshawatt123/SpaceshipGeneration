using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ShipBuilderWindow : EditorWindow
{
    private static GUIStyle myStyle;
    
    private static Camera FR = null;
    private RenderTexture FR_tex;
    
    private static Camera FL = null;
    private RenderTexture FL_tex;
    
    private static Camera RR = null;
    private RenderTexture RR_tex;
    
    private static Camera RL = null;
    private RenderTexture RL_tex;

    private static Camera F = null;
    private RenderTexture F_tex;

    private static Camera R = null;
    private RenderTexture R_tex;
    
    
    private static TreeGrammarV2 grammarControl = null;
    private static SymbolLookup grammarSymbols = null;

    private float cameraWidth = 200;
    private float cameraHeight = 200;
    private float xOffset = 0;
    private float yOffset = 200;

    [MenuItem("Window/ShipGen/ShipBuilder")]
    static void Init()
    {
        EditorWindow editorWindow = GetWindow(typeof(ShipBuilderWindow));
        editorWindow.autoRepaintOnSceneChange = true;
        editorWindow.Show();
        
        grammarControl = GameObject.FindObjectOfType<TreeGrammarV2>();
        grammarSymbols = GameObject.FindObjectOfType<SymbolLookup>();

        F = GameObject.Find("ShipRenderCam_F").GetComponent<Camera>();
        R = GameObject.Find("ShipRenderCam_R").GetComponent<Camera>();
        
        FR = GameObject.Find("ShipRenderCam_FR").GetComponent<Camera>();
        FL = GameObject.Find("ShipRenderCam_FL").GetComponent<Camera>();
        RR = GameObject.Find("ShipRenderCam_RR").GetComponent<Camera>();
        RL = GameObject.Find("ShipRenderCam_RL").GetComponent<Camera>();
    }

    public void Awake()
    {
        F_tex =  new RenderTexture((int)position.width, (int)position.height, (int)RenderTextureFormat.ARGB32);
        R_tex =  new RenderTexture((int)position.width, (int)position.height, (int)RenderTextureFormat.ARGB32);
        
        FR_tex = new RenderTexture((int)position.width, (int)position.height, (int)RenderTextureFormat.ARGB32);
        FL_tex = new RenderTexture((int)position.width, (int)position.height, (int)RenderTextureFormat.ARGB32);
        RR_tex = new RenderTexture((int)position.width, (int)position.height, (int)RenderTextureFormat.ARGB32);
        RL_tex = new RenderTexture((int)position.width, (int)position.height, (int)RenderTextureFormat.ARGB32);
    }

    private void Update()
    {
        UpdateTexture(F, ref F_tex);
        UpdateTexture(R, ref R_tex);
        
        UpdateTexture(FR, ref FR_tex);
        UpdateTexture(FL, ref FL_tex);
        UpdateTexture(RR, ref RR_tex);
        UpdateTexture(RL, ref RL_tex);
    }

    private void UpdateTexture(Camera camera, ref RenderTexture renderTexture)
    {
        if (camera != null)
        {
            camera.targetTexture = renderTexture;
            camera.Render();
            camera.targetTexture = null;
        }
        
        // resize the renderTexture 
        //if (renderTexture.width != position.width || renderTexture.height != position.height)
            //renderTexture = new RenderTexture((int)400, (int)400, (int)RenderTextureFormat.ARGB32);
    }

    private void OnGUI()
    {
        myStyle = new GUIStyle(GUI.skin.label);
        myStyle.margin = new RectOffset(10, 10, 20, 20);
        
        if (grammarControl)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Refresh"))
            {
                Init();
            }

            if (GUILayout.Button("Cleanup"))
            {
                grammarControl.Cleanup();
            }

            if (GUILayout.Button("Help"))
            {
                EditorWindow editorWindow = GetWindow(typeof(ShipBuilderHelpWindow));
                editorWindow.autoRepaintOnSceneChange = true;
                editorWindow.Show();
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Init ship gen"))
            {
                grammarSymbols.Init();
            }

            if (GUILayout.Button("Generate new ship"))
            {
                grammarControl.BuildShip();
            }

            grammarControl.showSymmetryBlocks =
                GUILayout.Toggle(grammarControl.showSymmetryBlocks, "Show symmetry blocks");
            GUILayout.EndHorizontal();

            GUILayout.Label("Camera Width: " + cameraWidth.ToString());
            cameraWidth = GUILayout.HorizontalSlider(cameraWidth, 100f, 1000f);
            GUILayout.Space(20);
            GUILayout.Label("Camera Height: " + cameraHeight.ToString());
            cameraHeight = GUILayout.HorizontalSlider(cameraHeight, 100f, 1000f);

            GUI.DrawTexture(new Rect(xOffset, yOffset, cameraWidth, cameraHeight), FL_tex);
            GUI.DrawTexture(new Rect(xOffset + cameraWidth, yOffset, cameraWidth, cameraHeight), FR_tex);
            GUI.DrawTexture(new Rect(xOffset, yOffset + cameraHeight, cameraWidth, cameraHeight), RL_tex);
            GUI.DrawTexture(new Rect(xOffset + cameraWidth, yOffset + cameraHeight, cameraWidth, cameraHeight), RR_tex);

            GUI.DrawTexture(new Rect(xOffset + cameraWidth * 2, yOffset, cameraWidth, cameraHeight), F_tex);
            GUI.DrawTexture(new Rect(xOffset + cameraWidth * 2, yOffset + cameraHeight, cameraWidth, cameraHeight),
                R_tex);

            //GUILayout.Label(position.width.ToString() + ":" + position.height.ToString());
        }
        else
        {
            if (GUILayout.Button("Refresh"))
            {
                Init();
            }
            GUILayout.Label("No TGV2 found in scene. Please place one :)");
            GUILayout.Label("It can be found in RadiatorGames/ShipBuilder/Prefabs");
        }
    }
}
