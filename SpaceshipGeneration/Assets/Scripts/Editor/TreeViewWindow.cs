using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TreeViewWindow : EditorWindow
{
    GameObject obj;
    GameObject prevFrameObj;
    private Tree objTree = null;

    Rect rootRect = new Rect(600, 0, 0, 0);

    Vector2 currMousePos;
    Vector2 prevMousePos;
    Vector2 deltaPos;
    Rect globalOffset = new Rect(0, 0, 0, 0);

    const float MAXOFFSET_X = 1000;
    const float MAXOFFSET_Y = 200;

    public Rect testWindow = new Rect(10, 10, 100, 50);

    private bool gotNodes = false;

    class WindowNode
    {
        public Rect windowRect;
        public string windowText;
        public Color color;
    }

    struct Line
    {
        public Vector3 from;
        public Vector3 to;
        public Color color;
    }

    List<WindowNode> windowRects = new List<WindowNode>();
    List<WindowNode> windowRectsCopy = new List<WindowNode>();
    List<Line> windowLinks = new List<Line>();


    [MenuItem("Window/Tree Viewer")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(TreeViewWindow));
    }

    private int x = 0;
    private int y = 0;
    private void OnGUI()
    {
        wantsMouseMove = true;

        // scroll the boxes
        Event e = Event.current;
        if(e.type == EventType.MouseDrag)
        {
            globalOffset.x += e.delta.x;
            globalOffset.y += e.delta.y;
        }


        // set default color to be safe
        GUI.color = Color.white;

        if (Selection.gameObjects.Length > 0)
            obj = Selection.gameObjects[0];
        else
            obj = null;

        // get tree
        if(obj)
        {
            if (obj.GetComponent<TreeGrammarBase>())
            {
                objTree = obj.GetComponent<TreeGrammarBase>().GetTree();
                //Debug.Log("Got tree");
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }

        if (objTree == null)
        {
            Debug.Log("Something fucked up");
            return;
        }

        if (!gotNodes)
        {
            // generate tree data
            if (objTree.root != null)
            {
                Debug.Log("Generating tree data");
                Node currNode = objTree.root;

                windowRects.Clear();

                // recursive!
                ExploreNode(currNode, 0, currNode.nodes.Count, 0.5f, ref rootRect);

                windowRectsCopy = windowRects;
            }

            gotNodes = true;
        }

        // draw lines and windows

        Handles.BeginGUI();
        foreach (Line line in windowLinks)
        {
            Handles.color = line.color;

            Vector3 fromTemp = line.from;
            Vector3 toTemp = line.to;

            fromTemp.x += globalOffset.x;
            fromTemp.y += globalOffset.y;
            toTemp.x += globalOffset.x;
            toTemp.y += globalOffset.y;

            Handles.DrawLine(fromTemp, toTemp);
        }
        Handles.EndGUI();


        BeginWindows();
  
        for(int i=0; i< windowRects.Count; i++)
        {
            GUI.color = windowRects[i].color;

            Rect tempRect = new Rect();
            tempRect.x = windowRects[i].windowRect.x + globalOffset.x;
            tempRect.y = windowRects[i].windowRect.y + globalOffset.y;
            tempRect.width = 50;
            tempRect.height = 50;

            GUI.Window(i, tempRect, WindowFunction, "X: " + windowRects[i].windowRect.x + "; Y: " + windowRects[i].windowRect.y);
        }

        //testWindow = GUI.Window(100, testWindow, WindowFunction, "X: " + windowRects[0].windowRect.x + "; Y: " + windowRects[0].windowRect.y + "\n" + "X: " + globalOffset.x + "; Y: " + globalOffset.y);

        EndWindows();

        Repaint();

    }
    void WindowFunction(int windowID)
    {
        GUI.DragWindow();
    }
    void ExploreNode(Node node, int depth, float siblings, float offsetAmount, ref Rect parent)
    {

        if (depth > 10)
            return;

        float offset = 0;
        // create rect in correct place
        if (depth == 0)
        {
            offset = MAXOFFSET_X;   
        }
        else
        {
            offset = MAXOFFSET_X * 1/(depth* 2);
        }
        Debug.Log(offset + "=" + MAXOFFSET_X + "*" + (depth == 0 ? 1 : 1/depth) + ":" + depth);
        //offset /= siblings;
        Rect rect = new Rect(parent.x + (offset * RadiatorGames.Math.Mapping.Map(0, siblings-1, -1, 1, offsetAmount)), parent.y + MAXOFFSET_Y, 80, 50);

        // add rect to list
        WindowNode windowNode = new WindowNode();
        windowNode.windowRect = rect;

        // set window text
        //windowNode.windowText += node.nodeType.ToString();
        if (node.name == "undefined")
        {
            windowNode.windowText += ("x:" + rect.x.ToString() + "\ny: " + rect.y.ToString());
        }
        else
        {
            windowNode.windowText += node.name;
        }

        windowRects.Add(windowNode);

        // set window color based on type
        windowNode.color = node.bgColor;

        // create line to parent

        // base case
        if (node.nodes.Count > 0)
        {
            int i = 0;
            foreach (Node child in node.nodes)
            {
                //float offsetStrength = (i + 1 / node.nodes.Count) - 1;

                ExploreNode(child, depth+1, node.nodes.Count, i, ref rect);
                i++;
            }

            //Debug.Log("Explored " + i + " nodes at level " + depth);
        }

        Line line = new Line();
        line.from = rect.center;
        line.to = parent.center;
        line.color = Color.red;


        //parent.x += offset / 3;


        windowLinks.Add(line);
    }

    float Map(float start, float a1, float a2, float b1, float b2)
    {
        return b1 + (((start - a1) * (b2 - b1)) / (a2 - a1));
    }
}